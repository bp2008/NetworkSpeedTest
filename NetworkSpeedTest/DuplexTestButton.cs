using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkSpeedTest
{
	public partial class DuplexTestButton : UserControl
	{
		public DuplexTestButton()
		{
			InitializeComponent();
		}

		private static SpeedTestForm speedTestForm = null;

		private void btnDuplexTest_Click(object sender, EventArgs e)
		{
			if (speedTestForm == null)
			{
				speedTestForm = new SpeedTestForm();
				speedTestForm.Show(this.ParentForm);
				speedTestForm.FormClosed += SpeedTestForm_FormClosed;
			}
			else
				speedTestForm.BringToFront();
		}
		private static void SpeedTestForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			speedTestForm = null;
		}
	}
}
