using BPUtil;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkSpeedTest.SpeedTest
{
	public class SpeedTestServer
	{
		private Thread listenThread;

		private TcpListener tcpListener;
		private UdpClient udpListener;
		private ConcurrentDictionary<string, ControlConnectionContext> currentControlConnections = new ConcurrentDictionary<string, ControlConnectionContext>();

		private GlobalUdpBroadcaster autodetectBroadcaster;
		private object autodetectBroadcasterLock = new object();

		public int tcpListenPort { get; private set; } = 0;
		public int udpListenPort { get; private set; } = 0;


		public SpeedTestServer()
		{
		}
		public void Start()
		{
			Stop();

			NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
			NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;

			NetworkAddressesChanged();

			listenThread = new Thread(listenManager);
			listenThread.Name = "SpeedTest Listen Thread";
			listenThread.IsBackground = true;
			listenThread.Start();
		}

		private void listenManager()
		{
			try
			{
				while (true)
				{
					try
					{
						tcpListenPort = 0;
						udpListenPort = 0;

						IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Any, 0);

						tcpListener = new TcpListener(localEndpoint);
						tcpListener.Start();
						try
						{
							tcpListener.BeginAcceptTcpClient(HandleTcpConnection, tcpListener);

							udpListener = new UdpClient(localEndpoint);
							udpListener.BeginReceive(HandleUdpPacket, udpListener);
							try
							{
								tcpListenPort = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
								udpListenPort = ((IPEndPoint)udpListener.Client.LocalEndPoint).Port;

								try
								{
									while (true)
										Thread.Sleep(30000);
								}
								finally
								{
									autodetectBroadcaster.Stop();
								}
							}
							finally
							{
								udpListener.Close();
							}
						}
						finally
						{
							tcpListener.Stop();
						}
					}
					catch (ThreadAbortException) { }
					catch (Exception ex)
					{
						tcpListenPort = 0;
						udpListenPort = 0;
						Logger.Debug(ex);
						Thread.Sleep(1000);
					}
				}
			}
			catch (ThreadAbortException) { }
			catch (Exception ex)
			{
				Logger.Debug(ex);
			}
		}

		public void Stop()
		{
			listenThread?.Abort();
			listenThread = null;

			try
			{
				NetworkChange.NetworkAvailabilityChanged -= NetworkChange_NetworkAvailabilityChanged;
				NetworkChange.NetworkAddressChanged -= NetworkChange_NetworkAddressChanged;
			}
			catch { }
		}

		#region Autodetect Broadcaster
		private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
		{
			NetworkAddressesChanged();
		}

		private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
		{
			NetworkAddressesChanged();
		}

		private void NetworkAddressesChanged()
		{
			lock (autodetectBroadcasterLock)
			{
				autodetectBroadcaster = new GlobalUdpBroadcaster(45678, true);
				autodetectBroadcaster.PacketReceived += AutodetectBroadcaster_PacketReceived;
			}
		}

		private Stopwatch broadcastTimer = Stopwatch.StartNew();
		private long lastBroadcastAt = -99999;
		private void AutodetectBroadcaster_PacketReceived(object sender, UdpPacket e)
		{
			try
			{
				string str = ByteUtil.ReadUtf8(e.Data);
				if (str == "SpeedTestServer Broadcast Request")
				{
					if (broadcastTimer.ElapsedMilliseconds - lastBroadcastAt > 1000) // Limit broadcasts to 1 per second
					{
						lastBroadcastAt = broadcastTimer.ElapsedMilliseconds;

						if (tcpListenPort > 0 && udpListenPort > 0)
						{
							byte[] autodetectBroadcastPacket = new byte["SpeedTestServer0".Length + 4];
							ByteUtil.WriteUtf8("SpeedTestServer0", autodetectBroadcastPacket, 0);
							ByteUtil.WriteUInt16((ushort)tcpListenPort, autodetectBroadcastPacket, autodetectBroadcastPacket.Length - 4);
							ByteUtil.WriteUInt16((ushort)udpListenPort, autodetectBroadcastPacket, autodetectBroadcastPacket.Length - 2);
							lock (autodetectBroadcasterLock)
								autodetectBroadcaster.Broadcast(autodetectBroadcastPacket);
						}
					}
				}
			}
			catch { }
		}
		#endregion

		#region TCP
		private void HandleTcpConnection(IAsyncResult ar)
		{
			TcpListener listener = (TcpListener)ar.AsyncState;
			TcpClient client = listener.EndAcceptTcpClient(ar);
			listener.BeginAcceptTcpClient(HandleTcpConnection, listener);
			HandleTcpClient(client);
		}

		private void HandleTcpClient(TcpClient client)
		{
			try
			{
				NetworkStream stream = client.GetStream();
				string first5Bytes = ByteUtil.ReadUtf8(stream, 6);
				if (first5Bytes == "STSCC0") // Control Connection
				{
					int testDirection = stream.ReadByte();

					// Generate a unique key for this control connection and tell it to the remote client.
					ControlConnectionContext control = new ControlConnectionContext(client, closingControlConnection =>
						{
							currentControlConnections.TryRemove(closingControlConnection.key, out ControlConnectionContext ignored);
						});
					control.testDirection = testDirection;
					currentControlConnections[control.key] = control;
					ByteUtil.WriteUtf8_16(control.key, stream);
					control.StartBandwidthMeasurement();
				}
				else if (first5Bytes == "STSST0") // TCP Speed Test connection
				{
					// Read the control connection unique key from the remote client.
					string ccKey = ByteUtil.ReadUtf8_16(stream);
					if (currentControlConnections.TryGetValue(ccKey, out ControlConnectionContext control))
					{
						TcpTestConnectionContext context = new TcpTestConnectionContext(control, stream, client, 0);
						// From remote perspective: 0: Download, 1: Upload, 2: Duplex
						if (control.testDirection == 0 || control.testDirection == 2)
							stream.BeginWrite(context.bufferToWrite, 0, context.bufferToWrite.Length, TcpTestConnectionClient.TcpWriteCallback, context);
						stream.BeginRead(context.bufferToRead, 0, context.bufferToRead.Length, TcpTestConnectionClient.TcpReadCallback, context);
					}
				}
				else
					client.Close();
			}
			catch
			{
				client.Close();
			}
		}

		#endregion

		#region UDP
		//private void udpListen()
		//{
		//	try
		//	{
		//		while (true)
		//		{
		//			try
		//			{
		//				IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Any, 0);
		//				using (UdpClient listener = new UdpClient(localEndpoint))
		//				{
		//					IAsyncResult asyncResult = listener.BeginReceive(HandleUdpPacket, listener);
		//				}
		//			}
		//			catch (ThreadAbortException) { }
		//			catch (Exception ex)
		//			{
		//				Logger.Debug(ex);
		//				Thread.Sleep(1000);
		//			}
		//		}
		//	}
		//	catch (ThreadAbortException) { }
		//	catch (Exception ex)
		//	{
		//		Logger.Debug(ex);
		//	}
		//}

		private void HandleUdpPacket(IAsyncResult result)
		{
			UdpClient client = (UdpClient)result.AsyncState;
			IPEndPoint remoteEndpoint = null;
			byte[] data = client.EndReceive(result, ref remoteEndpoint);
			udpListener.BeginReceive(HandleUdpPacket, udpListener);

			// Process the packet

		}
		#endregion
	}
}
