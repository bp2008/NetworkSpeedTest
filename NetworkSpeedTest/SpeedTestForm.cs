using BPUtil;
using BPUtil.Forms;
using NetworkSpeedTest.SpeedTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkSpeedTest
{
	public partial class SpeedTestForm : SelfCenteredForm
	{
		private BackgroundWorker bwSpeedTestController;
		private Autodetector autodetector;

		private string remoteHost;
		private int remoteTcpPort;
		private int remoteUdpPort;

		private long dlTotal = 0;
		private long ulTotal = 0;
		private Color originalListviewBack;

		public SpeedTestForm()
		{
			InitializeComponent();

			ServiceWrapper.PortsBound += ServiceWrapper_PortsBound;
			ServiceWrapper_PortsBound(null, null);

			autodetector = new Autodetector(this.lvRemoteHosts);
			autodetector.Start();

			if (Program.config.testDurationSeconds == 3)
				rbSeconds3.Checked = true;
			else if (Program.config.testDurationSeconds == 10)
				rbSeconds10.Checked = true;
			else if (Program.config.testDurationSeconds == 15)
				rbSeconds15.Checked = true;
			else
				rbSeconds5.Checked = true;

			if (Program.config.testWithTcp)
				rbTCP.Checked = true;
			else
				rbUDP.Checked = true;

			if (Program.config.testDirection == 0)
				rbDownloadTest.Checked = true;
			else if (Program.config.testDirection == 1)
				rbUploadTest.Checked = true;
			else
				rbDuplexTest.Checked = true;

			originalListviewBack = lvRemoteHosts.BackColor;
		}

		private void btnAddHost_Click(object sender, EventArgs e)
		{
			InputDialog txtInput = new InputDialog("Add Host", "Enter \"hostname:tcpPort:udpPort\"");
			if (txtInput.ShowDialog() == DialogResult.OK)
			{
				string[] parts = txtInput.InputText.Split(':');
				if (parts.Length == 3 && parts[0].Length > 0 && ushort.TryParse(parts[1], out ushort tcpPort) && ushort.TryParse(parts[2], out ushort udpPort))
					autodetector.AddManual(parts[0], tcpPort, udpPort);
				else
					MessageBox.Show("Invalid input.  Input must match the pattern \"hostname:tcpPort:udpPort\"");
			}
		}

		private void NewBackgroundWorker()
		{
			bwSpeedTestController = new BackgroundWorker();
			bwSpeedTestController.WorkerSupportsCancellation = true;
			bwSpeedTestController.WorkerReportsProgress = true;
			bwSpeedTestController.DoWork += BwSpeedTestController_DoWork;
			bwSpeedTestController.ProgressChanged += BwSpeedTestController_ProgressChanged;
			bwSpeedTestController.RunWorkerCompleted += BwSpeedTestController_RunWorkerCompleted;
		}

		private void SpeedTestForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			autodetector?.Stop();
			bwSpeedTestController?.CancelAsync();
			ServiceWrapper.PortsBound -= ServiceWrapper_PortsBound;
		}

		private void ServiceWrapper_PortsBound(object sender, EventArgs e)
		{
			if (ServiceWrapper.tcpPort == 0 && ServiceWrapper.udpPort == 0)
				SetTitle("Speed Test (local server not listening)");
			else
				SetTitle("Speed Test listening on tcp:" + ServiceWrapper.tcpPort + ", udp:" + ServiceWrapper.udpPort);
		}

		private void SetTitle(string str)
		{
			if (InvokeRequired)
				Invoke((Action<string>)SetTitle, str);
			else
				Text = str;
		}

		#region Form Input Handling
		private void rbSeconds3_CheckedChanged(object sender, EventArgs e)
		{
			Program.config.testDurationSeconds = 3;
			Program.config.Save();
		}

		private void rbSeconds5_CheckedChanged(object sender, EventArgs e)
		{
			Program.config.testDurationSeconds = 5;
			Program.config.Save();
		}

		private void rbSeconds10_CheckedChanged(object sender, EventArgs e)
		{
			Program.config.testDurationSeconds = 10;
			Program.config.Save();
		}

		private void rbSeconds15_CheckedChanged(object sender, EventArgs e)
		{
			Program.config.testDurationSeconds = 15;
			Program.config.Save();
		}

		private void nudUdpTestSpeed_ValueChanged(object sender, EventArgs e)
		{
			Program.config.udpTestSpeed = nudUdpTestSpeed.Value;
			Program.config.Save();
		}

		private void nudTcpStreams_ValueChanged(object sender, EventArgs e)
		{
			Program.config.tcpTestStreams = (int)nudTcpStreams.Value;
			Program.config.Save();
		}

		private void rbUDP_CheckedChanged(object sender, EventArgs e)
		{
			Program.config.testWithTcp = false;
			Program.config.Save();

			SetControlState(false);
		}

		private void rbTCP_CheckedChanged(object sender, EventArgs e)
		{
			Program.config.testWithTcp = true;
			Program.config.Save();

			SetControlState(false);
		}

		private void rbDuplexTest_CheckedChanged(object sender, EventArgs e)
		{
			Program.config.testDirection = 2;
			Program.config.Save();
		}

		private void rbDownloadTest_CheckedChanged(object sender, EventArgs e)
		{
			Program.config.testDirection = 0;
			Program.config.Save();
		}

		private void rbUploadTest_CheckedChanged(object sender, EventArgs e)
		{
			Program.config.testDirection = 1;
			Program.config.Save();
		}

		private void lvRemoteHosts_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lvRemoteHosts.SelectedItems.Count > 0)
			{
				RemoteSpeedTestServer server = (RemoteSpeedTestServer)lvRemoteHosts.SelectedItems[0].Tag;
				remoteHost = server.host;
				remoteTcpPort = server.tcpPort;
				remoteUdpPort = server.udpPort;
			}
			else
			{
				remoteHost = null;
				remoteTcpPort = 0;
				remoteUdpPort = 0;
			}
		}

		private void SetControlState(bool disableAll)
		{
			btnBeginTesting.Enabled = !disableAll;
			lvRemoteHosts.Enabled = !disableAll;
			rbSeconds3.Enabled = rbSeconds5.Enabled = rbSeconds10.Enabled = rbSeconds15.Enabled = !disableAll;
			rbDuplexTest.Enabled = rbDownloadTest.Enabled = rbUploadTest.Enabled = !disableAll;

			/*rbUDP.Enabled = */
			rbTCP.Enabled = !disableAll;
			rbUDP.Enabled = false; // TODO: Enable this

			autodetector.Enabled = !disableAll;

			nudTcpStreams.Enabled = Program.config.testWithTcp && !disableAll;
			nudUdpTestSpeed.Enabled = !Program.config.testWithTcp && !disableAll;
		}

		private void btnBeginTesting_Click(object sender, EventArgs e)
		{
			if (remoteHost == null)
			{
				Color notice = Color.Yellow;
				lvRemoteHosts.BackColor = notice;
				SetTimeout.OnGui(() => lvRemoteHosts.BackColor = originalListviewBack, 166, this);
				SetTimeout.OnGui(() => lvRemoteHosts.BackColor = notice, 333, this);
				SetTimeout.OnGui(() => lvRemoteHosts.BackColor = originalListviewBack, 500, this);
				return;
			}

			SetControlState(true);

			if (bwSpeedTestController != null && bwSpeedTestController.IsBusy)
			{
				bwSpeedTestController.CancelAsync();
				Thread.Sleep(100);
			}
			NewBackgroundWorker();

			nsgCurrent.SetDuration((uint)Program.config.testDurationSeconds);
			nsgCurrent.SetDownloadData(new KeyValuePair<double, double>[0]);
			nsgCurrent.SetUploadData(new KeyValuePair<double, double>[0]);
			dlTotal = 0;
			ulTotal = 0;
			bwSpeedTestController.RunWorkerAsync(bwSpeedTestController);
		}
		#endregion

		private void BwSpeedTestController_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker self = (BackgroundWorker)e.Argument;
			int testDurationSeconds = Program.config.testDurationSeconds;
			// Open Control Connection
			using (TcpClient controlConnection = new TcpClient(remoteHost, remoteTcpPort))
			using (Stream controlStream = controlConnection.GetStream())
			{
				self.ReportProgress(0, new { DataType = 'D', Bytes = 0, Index = 0 });
				self.ReportProgress(0, new { DataType = 'U', Bytes = 0, Index = 0 });

				ByteUtil.WriteUtf8("STSCC0", controlStream);
				controlStream.WriteByte((byte)Program.config.testDirection);
				string key = ByteUtil.ReadUtf8_16(controlStream);

				ControlConnectionContext control = new ControlConnectionContext(controlConnection, disconnectedControl => { }, key);
				control.testDirection = Program.config.testDirection;

				long lastBytesRead = 0;
				int dlReportIdx = 0;
				control.speedReport = (bytesReadSoFar) =>
					{
						if (self.CancellationPending)
							return;
						long bytesNew = bytesReadSoFar - Interlocked.Exchange(ref lastBytesRead, bytesReadSoFar);
						int idx = Interlocked.Increment(ref dlReportIdx);
						self.ReportProgress(0, new { DataType = 'D', Bytes = bytesNew, Index = idx });
					};

				control.StartBandwidthMeasurement();
				Stopwatch sw = Stopwatch.StartNew();

				int numStreams = BPMath.Clamp(Program.config.tcpTestStreams, 1, 1000);
				for (int i = 0; i < numStreams; i++)
					TcpTestConnectionClient.OpenTestConnection(control, testDurationSeconds);

				bool isDownloading = control.testDirection == 0 || control.testDirection == 2;
				bool isUploading = control.testDirection == 1 || control.testDirection == 2;

				// Begin reading speed reports from the remote client.  These are our upload speed reports.
				int expectedNumSpeedReports = testDurationSeconds * 10;
				if (isUploading)
				{
					long lastBytesWritten = 0;
					for (int ulReportIdx = 0; ulReportIdx < expectedNumSpeedReports; ulReportIdx++)
					{
						if (self.CancellationPending)
							return;
						long bytesWritten = ByteUtil.ReadInt64(controlStream);
						long bytesNew = bytesWritten - lastBytesWritten;
						lastBytesWritten = bytesWritten;
						self.ReportProgress(0, new { DataType = 'U', Bytes = bytesNew, Index = ulReportIdx + 1 });
					}
				}

				while (isDownloading && dlReportIdx < expectedNumSpeedReports && ((testDurationSeconds * 1200) - sw.ElapsedMilliseconds > 0))
				{
					if (self.CancellationPending)
						return;
					Thread.Sleep(10);
				}
			}
		}

		private void BwSpeedTestController_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			dynamic progress = e.UserState;
			double BytesPerSecond = (double)progress.Bytes * 10;
			double BitsPerSecond = BytesPerSecond * 8;
			double MegabitsPerSecond = BitsPerSecond / 1000000;

			if (progress.DataType == 'D')
			{
				nsgCurrent.AddDownloadRecord(((double)progress.Index / 10d), MegabitsPerSecond);
				lblCurrentDownload.Text = StringUtil.FormatNetworkBits((long)BitsPerSecond) + "ps";
				dlTotal += progress.Bytes;
			}
			else
			{
				nsgCurrent.AddUploadRecord(((double)progress.Index / 10d), MegabitsPerSecond);
				lblCurrentUpload.Text = StringUtil.FormatNetworkBits((long)BitsPerSecond) + "ps";
				ulTotal += progress.Bytes;
			}
		}

		private void BwSpeedTestController_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			lblCurrentDownload.Text = StringUtil.FormatNetworkBits((long)(8 * ((double)dlTotal / Program.config.testDurationSeconds))) + "ps";
			lblCurrentUpload.Text = StringUtil.FormatNetworkBits((long)(8 * ((double)ulTotal / Program.config.testDurationSeconds))) + "ps";

			Label lbl = new Label();
			lbl.AutoSize = false;
			lbl.Width = testHistory.Width - 30;
			lbl.Text = "[" + remoteHost + " " + (Program.config.testWithTcp ? "TCP" : "UDP") + "] " + lblCurrentDownload.Text + " down, " + lblCurrentUpload.Text + " up (at " + DateTime.Now.ToShortTimeString() + ")";
			testHistory.Controls.Add(lbl);

			NetworkSpeedGraph historyGraph = new NetworkSpeedGraph();
			historyGraph.LegendEnabled = false;
			historyGraph.SetDuration(nsgCurrent.GetDuration());
			historyGraph.SetDownloadData(nsgCurrent.GetDownloadData());
			historyGraph.SetUploadData(nsgCurrent.GetUploadData());
			historyGraph.Width = testHistory.Width - 30;
			testHistory.Controls.Add(historyGraph);

			SetControlState(false);
		}
	}
}
