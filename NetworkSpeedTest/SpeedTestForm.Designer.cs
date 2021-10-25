namespace NetworkSpeedTest
{
	partial class SpeedTestForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Test",
            "Test Host 1"}, -1);
			this.lvRemoteHosts = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.panel1 = new System.Windows.Forms.Panel();
			this.rbSeconds15 = new System.Windows.Forms.RadioButton();
			this.rbSeconds10 = new System.Windows.Forms.RadioButton();
			this.rbSeconds5 = new System.Windows.Forms.RadioButton();
			this.rbSeconds3 = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.btnBeginTesting = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.nudTcpStreams = new System.Windows.Forms.NumericUpDown();
			this.nudUdpTestSpeed = new System.Windows.Forms.NumericUpDown();
			this.rbUDP = new System.Windows.Forms.RadioButton();
			this.rbTCP = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.lblCurrentDownload = new System.Windows.Forms.Label();
			this.lblCurrentUpload = new System.Windows.Forms.Label();
			this.testHistory = new System.Windows.Forms.FlowLayoutPanel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.rbUploadTest = new System.Windows.Forms.RadioButton();
			this.rbDownloadTest = new System.Windows.Forms.RadioButton();
			this.rbDuplexTest = new System.Windows.Forms.RadioButton();
			this.nsgCurrent = new NetworkSpeedTest.NetworkSpeedGraph();
			this.btnAddHost = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudTcpStreams)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudUdpTestSpeed)).BeginInit();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// lvRemoteHosts
			// 
			this.lvRemoteHosts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lvRemoteHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvRemoteHosts.HideSelection = false;
			this.lvRemoteHosts.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.lvRemoteHosts.Location = new System.Drawing.Point(12, 7);
			this.lvRemoteHosts.MultiSelect = false;
			this.lvRemoteHosts.Name = "lvRemoteHosts";
			this.lvRemoteHosts.Size = new System.Drawing.Size(186, 137);
			this.lvRemoteHosts.TabIndex = 10;
			this.lvRemoteHosts.UseCompatibleStateImageBehavior = false;
			this.lvRemoteHosts.View = System.Windows.Forms.View.Details;
			this.lvRemoteHosts.SelectedIndexChanged += new System.EventHandler(this.lvRemoteHosts_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Remote Host:";
			this.columnHeader1.Width = 182;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.rbSeconds15);
			this.panel1.Controls.Add(this.rbSeconds10);
			this.panel1.Controls.Add(this.rbSeconds5);
			this.panel1.Controls.Add(this.rbSeconds3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Location = new System.Drawing.Point(12, 179);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(186, 40);
			this.panel1.TabIndex = 4;
			// 
			// rbSeconds15
			// 
			this.rbSeconds15.AutoSize = true;
			this.rbSeconds15.Location = new System.Drawing.Point(126, 19);
			this.rbSeconds15.Name = "rbSeconds15";
			this.rbSeconds15.Size = new System.Drawing.Size(37, 17);
			this.rbSeconds15.TabIndex = 26;
			this.rbSeconds15.TabStop = true;
			this.rbSeconds15.Text = "15";
			this.rbSeconds15.UseVisualStyleBackColor = true;
			this.rbSeconds15.CheckedChanged += new System.EventHandler(this.rbSeconds15_CheckedChanged);
			// 
			// rbSeconds10
			// 
			this.rbSeconds10.AutoSize = true;
			this.rbSeconds10.Location = new System.Drawing.Point(83, 19);
			this.rbSeconds10.Name = "rbSeconds10";
			this.rbSeconds10.Size = new System.Drawing.Size(37, 17);
			this.rbSeconds10.TabIndex = 24;
			this.rbSeconds10.TabStop = true;
			this.rbSeconds10.Text = "10";
			this.rbSeconds10.UseVisualStyleBackColor = true;
			this.rbSeconds10.CheckedChanged += new System.EventHandler(this.rbSeconds10_CheckedChanged);
			// 
			// rbSeconds5
			// 
			this.rbSeconds5.AutoSize = true;
			this.rbSeconds5.Location = new System.Drawing.Point(46, 19);
			this.rbSeconds5.Name = "rbSeconds5";
			this.rbSeconds5.Size = new System.Drawing.Size(31, 17);
			this.rbSeconds5.TabIndex = 22;
			this.rbSeconds5.TabStop = true;
			this.rbSeconds5.Text = "5";
			this.rbSeconds5.UseVisualStyleBackColor = true;
			this.rbSeconds5.CheckedChanged += new System.EventHandler(this.rbSeconds5_CheckedChanged);
			// 
			// rbSeconds3
			// 
			this.rbSeconds3.AutoSize = true;
			this.rbSeconds3.Location = new System.Drawing.Point(6, 19);
			this.rbSeconds3.Name = "rbSeconds3";
			this.rbSeconds3.Size = new System.Drawing.Size(31, 17);
			this.rbSeconds3.TabIndex = 20;
			this.rbSeconds3.TabStop = true;
			this.rbSeconds3.Text = "3";
			this.rbSeconds3.UseVisualStyleBackColor = true;
			this.rbSeconds3.CheckedChanged += new System.EventHandler(this.rbSeconds3_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(125, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Test Duration (Seconds):";
			// 
			// btnBeginTesting
			// 
			this.btnBeginTesting.Location = new System.Drawing.Point(12, 395);
			this.btnBeginTesting.Name = "btnBeginTesting";
			this.btnBeginTesting.Size = new System.Drawing.Size(186, 45);
			this.btnBeginTesting.TabIndex = 60;
			this.btnBeginTesting.Text = "Begin Testing";
			this.btnBeginTesting.UseVisualStyleBackColor = true;
			this.btnBeginTesting.Click += new System.EventHandler(this.btnBeginTesting_Click);
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.label5);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Controls.Add(this.nudTcpStreams);
			this.panel2.Controls.Add(this.nudUdpTestSpeed);
			this.panel2.Controls.Add(this.rbUDP);
			this.panel2.Controls.Add(this.rbTCP);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Location = new System.Drawing.Point(12, 225);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(186, 125);
			this.panel2.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(105, 44);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(33, 13);
			this.label5.TabIndex = 45;
			this.label5.Text = "Mbps";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(80, 99);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(43, 13);
			this.label4.TabIndex = 44;
			this.label4.Text = "streams";
			// 
			// nudTcpStreams
			// 
			this.nudTcpStreams.Location = new System.Drawing.Point(8, 97);
			this.nudTcpStreams.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudTcpStreams.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudTcpStreams.Name = "nudTcpStreams";
			this.nudTcpStreams.Size = new System.Drawing.Size(69, 20);
			this.nudTcpStreams.TabIndex = 43;
			this.nudTcpStreams.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudTcpStreams.ValueChanged += new System.EventHandler(this.nudTcpStreams_ValueChanged);
			// 
			// nudUdpTestSpeed
			// 
			this.nudUdpTestSpeed.DecimalPlaces = 3;
			this.nudUdpTestSpeed.Location = new System.Drawing.Point(8, 42);
			this.nudUdpTestSpeed.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.nudUdpTestSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudUdpTestSpeed.Name = "nudUdpTestSpeed";
			this.nudUdpTestSpeed.Size = new System.Drawing.Size(91, 20);
			this.nudUdpTestSpeed.TabIndex = 33;
			this.nudUdpTestSpeed.Value = new decimal(new int[] {
            1200,
            0,
            0,
            0});
			this.nudUdpTestSpeed.ValueChanged += new System.EventHandler(this.nudUdpTestSpeed_ValueChanged);
			// 
			// rbUDP
			// 
			this.rbUDP.AutoSize = true;
			this.rbUDP.Location = new System.Drawing.Point(6, 19);
			this.rbUDP.Name = "rbUDP";
			this.rbUDP.Size = new System.Drawing.Size(48, 17);
			this.rbUDP.TabIndex = 30;
			this.rbUDP.TabStop = true;
			this.rbUDP.Text = "UDP";
			this.rbUDP.UseVisualStyleBackColor = true;
			this.rbUDP.CheckedChanged += new System.EventHandler(this.rbUDP_CheckedChanged);
			// 
			// rbTCP
			// 
			this.rbTCP.AutoSize = true;
			this.rbTCP.Location = new System.Drawing.Point(6, 74);
			this.rbTCP.Name = "rbTCP";
			this.rbTCP.Size = new System.Drawing.Size(46, 17);
			this.rbTCP.TabIndex = 40;
			this.rbTCP.TabStop = true;
			this.rbTCP.Text = "TCP";
			this.rbTCP.UseVisualStyleBackColor = true;
			this.rbTCP.CheckedChanged += new System.EventHandler(this.rbTCP_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "Test Configuration:";
			// 
			// lblCurrentDownload
			// 
			this.lblCurrentDownload.AutoSize = true;
			this.lblCurrentDownload.BackColor = System.Drawing.Color.Black;
			this.lblCurrentDownload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.lblCurrentDownload.Location = new System.Drawing.Point(508, 60);
			this.lblCurrentDownload.Name = "lblCurrentDownload";
			this.lblCurrentDownload.Size = new System.Drawing.Size(42, 13);
			this.lblCurrentDownload.TabIndex = 151;
			this.lblCurrentDownload.Text = "0 Mbps";
			// 
			// lblCurrentUpload
			// 
			this.lblCurrentUpload.AutoSize = true;
			this.lblCurrentUpload.BackColor = System.Drawing.Color.Black;
			this.lblCurrentUpload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
			this.lblCurrentUpload.Location = new System.Drawing.Point(508, 82);
			this.lblCurrentUpload.Name = "lblCurrentUpload";
			this.lblCurrentUpload.Size = new System.Drawing.Size(42, 13);
			this.lblCurrentUpload.TabIndex = 152;
			this.lblCurrentUpload.Text = "0 Mbps";
			// 
			// testHistory
			// 
			this.testHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.testHistory.AutoScroll = true;
			this.testHistory.BackColor = System.Drawing.SystemColors.Control;
			this.testHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.testHistory.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
			this.testHistory.Location = new System.Drawing.Point(204, 123);
			this.testHistory.Name = "testHistory";
			this.testHistory.Size = new System.Drawing.Size(395, 318);
			this.testHistory.TabIndex = 153;
			this.testHistory.WrapContents = false;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.rbUploadTest);
			this.panel3.Controls.Add(this.rbDownloadTest);
			this.panel3.Controls.Add(this.rbDuplexTest);
			this.panel3.Location = new System.Drawing.Point(12, 360);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(186, 24);
			this.panel3.TabIndex = 50;
			// 
			// rbUploadTest
			// 
			this.rbUploadTest.AutoSize = true;
			this.rbUploadTest.Location = new System.Drawing.Point(144, 3);
			this.rbUploadTest.Name = "rbUploadTest";
			this.rbUploadTest.Size = new System.Drawing.Size(39, 17);
			this.rbUploadTest.TabIndex = 53;
			this.rbUploadTest.TabStop = true;
			this.rbUploadTest.Text = "Up";
			this.rbUploadTest.UseVisualStyleBackColor = true;
			this.rbUploadTest.CheckedChanged += new System.EventHandler(this.rbUploadTest_CheckedChanged);
			// 
			// rbDownloadTest
			// 
			this.rbDownloadTest.AutoSize = true;
			this.rbDownloadTest.Location = new System.Drawing.Point(75, 3);
			this.rbDownloadTest.Name = "rbDownloadTest";
			this.rbDownloadTest.Size = new System.Drawing.Size(53, 17);
			this.rbDownloadTest.TabIndex = 52;
			this.rbDownloadTest.TabStop = true;
			this.rbDownloadTest.Text = "Down";
			this.rbDownloadTest.UseVisualStyleBackColor = true;
			this.rbDownloadTest.CheckedChanged += new System.EventHandler(this.rbDownloadTest_CheckedChanged);
			// 
			// rbDuplexTest
			// 
			this.rbDuplexTest.AutoSize = true;
			this.rbDuplexTest.Location = new System.Drawing.Point(6, 3);
			this.rbDuplexTest.Name = "rbDuplexTest";
			this.rbDuplexTest.Size = new System.Drawing.Size(58, 17);
			this.rbDuplexTest.TabIndex = 51;
			this.rbDuplexTest.TabStop = true;
			this.rbDuplexTest.Text = "Duplex";
			this.rbDuplexTest.UseVisualStyleBackColor = true;
			this.rbDuplexTest.CheckedChanged += new System.EventHandler(this.rbDuplexTest_CheckedChanged);
			// 
			// nsgCurrent
			// 
			this.nsgCurrent.LegendEnabled = true;
			this.nsgCurrent.Location = new System.Drawing.Point(204, 7);
			this.nsgCurrent.Name = "nsgCurrent";
			this.nsgCurrent.Size = new System.Drawing.Size(400, 110);
			this.nsgCurrent.TabIndex = 100;
			// 
			// btnAddHost
			// 
			this.btnAddHost.Location = new System.Drawing.Point(12, 150);
			this.btnAddHost.Name = "btnAddHost";
			this.btnAddHost.Size = new System.Drawing.Size(186, 23);
			this.btnAddHost.TabIndex = 154;
			this.btnAddHost.Text = "Add Host";
			this.btnAddHost.UseVisualStyleBackColor = true;
			this.btnAddHost.Click += new System.EventHandler(this.btnAddHost_Click);
			// 
			// SpeedTestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(611, 452);
			this.Controls.Add(this.btnAddHost);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.testHistory);
			this.Controls.Add(this.lblCurrentUpload);
			this.Controls.Add(this.lblCurrentDownload);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.btnBeginTesting);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lvRemoteHosts);
			this.Controls.Add(this.nsgCurrent);
			this.MaximumSize = new System.Drawing.Size(627, 999999);
			this.MinimumSize = new System.Drawing.Size(627, 39);
			this.Name = "SpeedTestForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "SpeedTestForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpeedTestForm_FormClosing);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudTcpStreams)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudUdpTestSpeed)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private NetworkSpeedGraph nsgCurrent;
		private System.Windows.Forms.ListView lvRemoteHosts;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton rbSeconds15;
		private System.Windows.Forms.RadioButton rbSeconds10;
		private System.Windows.Forms.RadioButton rbSeconds5;
		private System.Windows.Forms.RadioButton rbSeconds3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnBeginTesting;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.NumericUpDown nudUdpTestSpeed;
		private System.Windows.Forms.RadioButton rbUDP;
		private System.Windows.Forms.RadioButton rbTCP;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudTcpStreams;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblCurrentDownload;
		private System.Windows.Forms.Label lblCurrentUpload;
		private System.Windows.Forms.FlowLayoutPanel testHistory;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.RadioButton rbUploadTest;
		private System.Windows.Forms.RadioButton rbDownloadTest;
		private System.Windows.Forms.RadioButton rbDuplexTest;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Button btnAddHost;
	}
}