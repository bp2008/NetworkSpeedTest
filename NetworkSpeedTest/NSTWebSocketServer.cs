using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BPUtil;
using WebSocketSharp;
using WebSocketSharp.Server;
using Logger = BPUtil.Logger;

namespace NetworkSpeedTest
{
	public class NSTWebSocketServer
	{
		private const string myPath = "/nstws";
		WebSocketServer srv;
		public NSTWebSocketServer(int port)
		{
			srv = new WebSocketServer(IPAddress.None, port, false);
			srv.Log.Output = (LogData data, string path) =>
			{
				Console.WriteLine(data.Message);
			};
			srv.AddWebSocketService<NSTWebSocketBehavior>(myPath);
		}

		/// <summary>
		/// </summary>
		/// <param name="connectionKey"></param>
		public void AcceptIncomingConnection(TcpClient tcpc)
		{
			tcpc.NoDelay = true;
			//tcpc.GetStream()
			srv.AcceptTcpClient(tcpc);
			while (tcpc.Connected)
				Thread.Sleep(100);
		}

		public void Start()
		{
			srv.Start();
		}
		public void Stop()
		{
			Try.Catch(() => { srv.Stop(); });
		}
		private class NSTWebSocketBehavior : WebSocketBehavior
		{
			private bool calledClose = false;
			private object closeLock = new object();

			public NSTWebSocketBehavior() : base()
			{
			}
			private void CloseSocket()
			{
				if (calledClose)
					return;
				lock (closeLock)
				{
					if (calledClose)
						return;
					calledClose = true;
				}
				Sessions.CloseSession(ID);
			}

			private void StopStreaming(int waitMs = 10000)
			{

			}

			protected override void OnClose(CloseEventArgs e)
			{
				StopStreaming();
			}
			protected override void OnMessage(MessageEventArgs e)
			{
				if (!e.IsBinary)
				{
					CloseSocket();
					return;
				}

				byte[] buf = e.RawData;
				Console.WriteLine(ID + " OnMessage(" + buf.Length + ")");
				if (buf.Length == 0)
				{
					CloseSocket();
					return;
				}

				//Command cmd = (Command)buf[0];
				//Console.WriteLine(ID + " OnMessage: " + cmd);
				//try
				//{
				//	switch (cmd)
				//	{
				//		default:
				//			// CloseSocket();
				//			Send(new byte[1]);
				//			break;
				//	}
				//}
				//catch (ThreadAbortException) { throw; }
				//catch (Exception ex)
				//{
				//	Logger.Debug(ex, "WebSocketServer");
				//}
			}
		}
	}
}
