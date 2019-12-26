using BPUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkSpeedTest.SpeedTest
{
	public class Autodetector
	{
		private UdpBroadcaster broadcastReceiver;
		private List<RemoteSpeedTestServer> servers = new List<RemoteSpeedTestServer>();
		private ListView lvServers;
		private Dictionary<RemoteSpeedTestServer, ListViewItem> serverToListViewItem = new Dictionary<RemoteSpeedTestServer, ListViewItem>();

		public Autodetector(ListView lvServers)
		{
			this.lvServers = lvServers;
			lvServers.Clear();
		}

		private void BroadcastReceiver_PacketReceived(object sender, UdpPacket e)
		{
			if (e.Data.Length > 16)
			{
				string introductionStr = ByteUtil.ReadUtf8(e.Data, 0, 16);
				if (introductionStr == "SpeedTestServer0")
				{
					if (e.Data.Length == 20)
					{
						string host = e.Sender.Address.ToString();
						ushort tcpPort = ByteUtil.ReadUInt16(e.Data, 16);
						ushort udpPort = ByteUtil.ReadUInt16(e.Data, 18);

						lock (servers)
						{
							RemoteSpeedTestServer server = servers.Find(s => s.host == host && s.tcpPort == tcpPort && s.udpPort == udpPort);
							if (server == null)
							{
								server = new RemoteSpeedTestServer(host, tcpPort, udpPort);
								servers.Add(server);
							}
							else
								server.time = DateTime.Now;
						}
						ServerListUpdated();
					}
				}
			}
		}

		private void ServerListUpdated()
		{
			if (lvServers.InvokeRequired)
				lvServers.Invoke((Action)ServerListUpdated);
			else
			{
				foreach (RemoteSpeedTestServer server in servers)
				{
					if (!serverToListViewItem.TryGetValue(server, out ListViewItem existingListItem))
					{
						serverToListViewItem[server] = existingListItem = new ListViewItem();
						existingListItem.Tag = server;
						lvServers.Items.Add(existingListItem);
						if (lvServers.Items.Count == 1)
							existingListItem.Selected = true;
					}
					existingListItem.Text = server.ToString();
				}
			}
		}

		public void Start()
		{
			broadcastReceiver = new UdpBroadcaster(45678, true);
			broadcastReceiver.PacketReceived += BroadcastReceiver_PacketReceived;
		}
		public void Stop()
		{
			broadcastReceiver?.Stop();
			broadcastReceiver = null;
		}
	}
}
