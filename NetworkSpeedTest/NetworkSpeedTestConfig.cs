using BPUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkSpeedTest
{
	public class NetworkSpeedTestConfig : SerializableObjectBase
	{
		public ushort port = 45678;
		public int testDurationSeconds = 5;
		public decimal udpTestSpeed = 1200;
		public int tcpTestStreams = 1;
		public bool testWithTcp = true;
		public int testDirection = 2;
		private static readonly string configFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/NetworkSpeedTest/Config.xml";
		public void Load()
		{
			this.Load(configFile);
		}
		public void Save()
		{
			this.Save(configFile);
		}
		public void SaveIfNoExist()
		{
			this.SaveIfNoExist(configFile);
		}
	}
}
