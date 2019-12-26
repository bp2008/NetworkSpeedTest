using BPUtil;
using System.Diagnostics;
using System.Net.Sockets;

namespace NetworkSpeedTest.SpeedTest
{
	internal class TcpTestConnectionContext
	{
		public ControlConnectionContext control;
		public NetworkStream testStream;
		public TcpClient testClient;
		public int testDurationSeconds;
		public Stopwatch testTimer = new Stopwatch();
		public byte[] bufferToWrite;
		public byte[] bufferToRead;

		public TcpTestConnectionContext(ControlConnectionContext control, NetworkStream testStream, TcpClient testClient, int testDurationSeconds)
		{
			this.control = control;
			this.testStream = testStream;
			this.testClient = testClient;
			this.testDurationSeconds = testDurationSeconds;
			this.bufferToWrite = StaticRandom.NextBytes(500000);
			this.bufferToRead = new byte[500000];
		}
	}
}
