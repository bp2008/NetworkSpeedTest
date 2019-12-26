using BPUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkSpeedTest.SpeedTest
{
	internal static class TcpTestConnectionClient
	{

		public static void OpenTestConnection(ControlConnectionContext control, int testDurationSeconds)
		{
			TcpClient testClient = null;
			try
			{
				IPEndPoint remoteEP = (IPEndPoint)control.client.Client.RemoteEndPoint;
				testClient = new TcpClient();
				TcpTestConnectionContext context = new TcpTestConnectionContext(control, null, testClient, testDurationSeconds);
				testClient.BeginConnect(remoteEP.Address, remoteEP.Port, EndConnect, context);
			}
			catch
			{
				testClient?.Close();
			}
		}

		private static void EndConnect(IAsyncResult ar)
		{
			TcpTestConnectionContext context = (TcpTestConnectionContext)ar.AsyncState;
			try
			{
				context.testClient.EndConnect(ar);
				context.testStream = context.testClient.GetStream();

				ByteUtil.WriteUtf8("STSST0", context.testStream);
				ByteUtil.WriteUtf8_16(context.control.key, context.testStream);

				if (context.control.testDirection == 1 || context.control.testDirection == 2)
					context.testStream.BeginWrite(context.bufferToWrite, 0, context.bufferToWrite.Length, TcpWriteCallback, context);
				context.testStream.BeginRead(context.bufferToRead, 0, context.bufferToRead.Length, TcpReadCallback, context);
			}
			catch
			{
				context.testClient.Close();
			}
		}

		internal static void TcpReadCallback(IAsyncResult ar)
		{
			TcpTestConnectionContext context = (TcpTestConnectionContext)ar.AsyncState;
			try
			{
				int read = context.testStream.EndRead(ar);
				if (read <= 0)
				{
					context.testClient.Close();
				}
				else
				{
					context.testStream.BeginRead(context.bufferToRead, 0, context.bufferToRead.Length, TcpReadCallback, context);
					context.control.CountDataRead(read);
				}
			}
			catch
			{
				context.testClient.Close();
			}
		}

		internal static void TcpWriteCallback(IAsyncResult ar)
		{
			TcpTestConnectionContext context = (TcpTestConnectionContext)ar.AsyncState;
			try
			{
				context.testStream.EndWrite(ar);
				if (context.testDurationSeconds > 0 && context.testTimer.ElapsedMilliseconds >= context.testDurationSeconds * 1000)
				{
					context.testClient.Close();
				}
				else
				{
					context.testStream.BeginWrite(context.bufferToWrite, 0, context.bufferToWrite.Length, TcpWriteCallback, context);
					if (!context.control.CheckConnected())
						context.testClient.Close();
				}
			}
			catch
			{
				context.testClient.Close();
			}
		}
	}
}
