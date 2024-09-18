using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NetworkSpeedTest
{
	partial class MainSvc : ServiceBase
	{
		public MainSvc()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			ServiceWrapper.Start();
		}

		protected override void OnStop()
		{
			ServiceWrapper.Stop();
		}
	}
}
