using BPUtil;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace NetworkSpeedTest.SpeedTest
{
	internal class ControlConnectionContext
	{
		public readonly string key;
		public TcpClient client;
		public NetworkStream stream;

		private object dataReadLock = new object();
		private long dataRead = 0;
		private Stopwatch sw = new Stopwatch();
		private int lastReportedTimeInterval = 0;
		private Action<ControlConnectionContext> disconnectCallback;
		/// <summary>
		/// If set, this Action will be called in order to report changes in speed.
		/// If not set, speed reports will be written to the control connection's network stream.
		/// </summary>
		public Action<long> speedReport = null;

		/// <summary>
		/// From Client Perspective: 0: Download, 1: Upload, 2: Duplex
		/// </summary>
		public int testDirection = 2;

		public ControlConnectionContext(TcpClient client, Action<ControlConnectionContext> disconnectCallback, string key = null)
		{
			if (key != null)
				this.key = key;
			else
				this.key = Guid.NewGuid().ToString();
			this.client = client;
			this.stream = client.GetStream();
			this.disconnectCallback = disconnectCallback;
		}

		public void CountDataRead(long read)
		{
			int thisInterval = (int)sw.ElapsedMilliseconds / 100;
			if (thisInterval > lastReportedTimeInterval)
				lock (dataReadLock)
				{
					while (thisInterval > lastReportedTimeInterval)
					{
						Interlocked.Increment(ref lastReportedTimeInterval);
						long readSoFar = Interlocked.Read(ref dataRead);
						if (speedReport != null)
							speedReport(readSoFar);
						else
						{
							try
							{
								byte[] buf = new byte[8];
								ByteUtil.WriteInt64(readSoFar, buf, 0);
								stream.BeginWrite(buf, 0, 8, SpeedReportCallback, null);
							}
							catch
							{
								client.Close();
								disconnectCallback(this);
							}
						}
					}
				}
			Interlocked.Add(ref dataRead, read);
		}

		private void SpeedReportCallback(IAsyncResult ar)
		{
			try
			{
				stream.EndWrite(ar);
			}
			catch
			{
				client.Close();
				disconnectCallback(this);
			}
		}
		public bool CheckConnected()
		{
			if (client.Connected)
				return true;
			disconnectCallback(this);
			return false;
		}

		public void StartBandwidthMeasurement()
		{
			sw.Start();
		}
	}
}
