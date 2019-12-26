using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NetworkSpeedTest
{
	public partial class NetworkSpeedGraph : UserControl
	{
		public NetworkSpeedGraph()
		{
			InitializeComponent();
			_legendEnabled = this.chart1.Legends.Any(l => l.Enabled);
			SetDuration(5);
		}

		private bool _legendEnabled = false;

		/// <summary>
		/// Shows or hides the legend.
		/// </summary>
		public bool LegendEnabled
		{
			get
			{
				return _legendEnabled;
			}
			set
			{
				EnableLegend(value);
			}
		}

		private uint _duration = 5;
		/// <summary>
		/// Sets the visible graph duration in seconds.
		/// </summary>
		/// <param name="seconds"></param>
		public void SetDuration(uint seconds)
		{
			if (this.chart1.InvokeRequired)
				this.chart1.BeginInvoke((Action<uint>)SetDuration, seconds);
			else
			{
				_duration = Math.Max(0, seconds);
				this.chart1.ChartAreas[0].AxisX.Minimum = 0;
				this.chart1.ChartAreas[0].AxisX.Maximum = _duration;
			}
		}

		public uint GetDuration()
		{
			return _duration;
		}

		/// <summary>
		/// Adds one record to the graph's "Download" series.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void AddDownloadRecord(double x, double y)
		{
			if (this.chart1.InvokeRequired)
				this.chart1.BeginInvoke((Action<double, double>)AddDownloadRecord, x, y);
			else
				DownloadData.AddXY(x, y);
		}

		/// <summary>
		/// Adds one record to the graph's "Upload" series.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void AddUploadRecord(double x, double y)
		{
			if (this.chart1.InvokeRequired)
				this.chart1.BeginInvoke((Action<double, double>)AddUploadRecord, x, y);
			else
				UploadData.AddXY(x, y);
		}

		/// <summary>
		/// Sets the "Download" series values.
		/// </summary>
		/// <param name="data"></param>
		public void SetDownloadData(KeyValuePair<double, double>[] data)
		{
			if (this.chart1.InvokeRequired)
				this.chart1.BeginInvoke((Action<KeyValuePair<double, double>[]>)SetDownloadData, data);
			else
			{
				DownloadData.Clear();
				foreach (KeyValuePair<double, double> record in data)
					DownloadData.AddXY(record.Key, record.Value);
			}
		}

		/// <summary>
		/// Sets the "Upload" series values.
		/// </summary>
		/// <param name="data"></param>
		public void SetUploadData(KeyValuePair<double, double>[] data)
		{
			if (this.chart1.InvokeRequired)
				this.chart1.BeginInvoke((Action<KeyValuePair<double, double>[]>)SetUploadData, data);
			else
			{
				UploadData.Clear();
				foreach (KeyValuePair<double, double> record in data)
					UploadData.AddXY(record.Key, record.Value);
			}
		}

		private DataPointCollection DownloadData
		{
			get
			{
				return this.chart1.Series[0].Points;
			}
		}

		private DataPointCollection UploadData
		{
			get
			{
				return this.chart1.Series[1].Points;
			}
		}

		public KeyValuePair<double, double>[] GetDownloadData()
		{
			return GetDataArray(DownloadData);
		}

		public KeyValuePair<double, double>[] GetUploadData()
		{
			return GetDataArray(UploadData);
		}

		private KeyValuePair<double, double>[] GetDataArray(DataPointCollection points)
		{
			return points.Select(p => new KeyValuePair<double, double>(p.XValue, p.YValues[0])).OrderBy(kvp => kvp.Key).ToArray();
		}

		private void EnableLegend(bool enable)
		{
			if (this.chart1.InvokeRequired)
				this.chart1.BeginInvoke((Action<bool>)EnableLegend, enable);
			else
				foreach (Legend l in this.chart1.Legends)
					l.Enabled = enable;
		}
	}
}
