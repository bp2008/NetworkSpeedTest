using System;

namespace NetworkSpeedTest.SpeedTest
{
	public class RemoteSpeedTestServer : IComparable<RemoteSpeedTestServer>, IComparable
	{
		public readonly string host;
		public readonly ushort tcpPort;
		public readonly ushort udpPort;
		public DateTime time = DateTime.Now;

		public RemoteSpeedTestServer(string host, ushort tcpPort, ushort udpPort)
		{
			this.host = host;
			this.tcpPort = tcpPort;
			this.udpPort = udpPort;
		}

		public int CompareTo(RemoteSpeedTestServer other)
		{
			if (other == null)
				return -1;
			int diff = host.CompareTo(other.host);
			if (diff == 0)
			{
				diff = tcpPort.CompareTo(other.tcpPort);
				if (diff == 0)
				{
					diff = udpPort.CompareTo(other.udpPort);
				}
			}
			return diff;
		}

		public int CompareTo(object obj)
		{
			if (obj is RemoteSpeedTestServer)
				return CompareTo(obj as RemoteSpeedTestServer);
			return -1;
		}
		public override bool Equals(object obj)
		{
			return this.CompareTo(obj) == 0;
		}
		public override int GetHashCode()
		{
			return host.GetHashCode() ^ tcpPort.GetHashCode() ^ udpPort.GetHashCode();
		}
		public override string ToString()
		{
			return host + " (Age: " + (DateTime.Now - time).TotalSeconds.ToString("0") + ")";
		}
	}
}
