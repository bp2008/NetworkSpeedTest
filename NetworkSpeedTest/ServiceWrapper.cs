using NetworkSpeedTest.SpeedTest;
using System;
using System.Threading;

namespace NetworkSpeedTest
{
	public static class ServiceWrapper
	{
		private static WebServer ws;
		private static SpeedTestServer sts;
		/// <summary>
		/// TCP port of the speed test server.
		/// </summary>
		public static int tcpPort
		{
			get
			{
				if (sts != null)
					return sts.tcpListenPort;
				return 0;
			}
		}
		/// <summary>
		/// UDP port of the speed test server.
		/// </summary>
		public static int udpPort
		{
			get
			{
				if (sts != null)
					return sts.udpListenPort;
				return 0;
			}
		}

		/// <summary>
		/// Gets a value indicating if the ServiceWrapper has been started and not yet stopped.
		/// </summary>
		public static bool Started { get { return sts != null; } }

		/// <summary>
		/// Event raised when tcp and udp port numbers are updated.
		/// </summary>
		public static event EventHandler PortsBound = delegate { };
		public static void Start()
		{
			if (ws != null)
			{
				Stop();
				Thread.Sleep(500);
			}
			ws = new WebServer();
			ws.SetBindings(Program.config.port, Program.config.port);

			sts = new SpeedTestServer();
			sts.PortsBound += Sts_PortsBound;
			sts.Start();
		}

		private static void Sts_PortsBound(object sender, EventArgs e)
		{
			PortsBound(sender, e);
		}

		public static void Stop()
		{
			if (ws != null)
			{
				ws.Stop();
				ws = null;
			}
			if (sts != null)
			{
				sts.PortsBound -= Sts_PortsBound;
				sts.Stop();
				sts = null;
			}
		}
	}
}