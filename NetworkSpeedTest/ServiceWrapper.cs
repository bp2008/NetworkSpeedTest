using NetworkSpeedTest.SpeedTest;
using System;
using System.Threading;

namespace NetworkSpeedTest
{
	public static class ServiceWrapper
	{
		private static WebServer ws;
		private static SpeedTestServer sts;
		public static void Start()
		{
			if (ws != null)
			{
				Stop();
				Thread.Sleep(500);
			}
			ws = new WebServer(Program.config.port);
			ws.Start();

			sts = new SpeedTestServer();
			sts.Start();
		}

		public static void Stop()
		{
			if (ws != null)
			{
				ws.Stop();
				ws = null;
			}

			sts.Stop();
		}
	}
}