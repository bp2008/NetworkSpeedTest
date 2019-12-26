using BPUtil;
using BPUtil.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkSpeedTest
{
	class Program
	{
		public static NetworkSpeedTestConfig config = new NetworkSpeedTestConfig();
		static void Main(string[] args)
		{
			Application.ThreadException += Application_ThreadException;
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

			config.Load();
			config.SaveIfNoExist();

			if (Environment.UserInteractive)
			{
				string Title = "Network Speed Test " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " Service Manager";
				string ServiceName = "NetworkSpeedTest";

				ButtonDefinition btnStartTemporary = new ButtonDefinition("Start Temporary Instance", btnStartTemporary_Click);
				ButtonDefinition btnPort = new ButtonDefinition("Web Port: " + config.port, btnPort_Click);

#if DEBUG
				if (System.Diagnostics.Debugger.IsAttached)
				{
					btnStartTemporary.Text = "Stop Temporary Instance";
					ServiceWrapper.Start();
				}
#endif

				ButtonDefinition[] customButtons = new ButtonDefinition[] { btnStartTemporary, btnPort };



				System.Windows.Forms.Application.Run(new ServiceManager(Title, ServiceName, customButtons, new DuplexTestButton()));
			}
			else
			{
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[]
				{
					new MainSvc()
				};
				ServiceBase.Run(ServicesToRun);
			}
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			Logger.Debug(e.Exception);
		}

		private static void btnPort_Click(object sender, EventArgs e)
		{
			InputDialog id = new InputDialog("Web Server Port", "Web Port [0-65535]:", config.port.ToString());
			if (id.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if (ushort.TryParse(id.InputText, out ushort port))
				{
					config.port = port;
					config.Save();

					((Button)sender).Text = "Port: " + port;
					MessageBox.Show("The web server port number has been changed to " + port + "." + Environment.NewLine + Environment.NewLine
						+ "Please restart the service or temporary instance for the change to take effect.");
				}
				else
					MessageBox.Show("Invalid Input. The web server port has not been changed.");
			}
		}
		private static void btnStartTemporary_Click(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			if (btn.Text.StartsWith("Start"))
			{
				ServiceWrapper.Start();
				btn.Text = "Stop Temporary Instance";
			}
			else
			{
				ServiceWrapper.Stop();
				btn.Text = "Start Temporary Instance";
			}
		}
	}
}