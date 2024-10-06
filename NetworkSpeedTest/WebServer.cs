using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BPUtil;
using BPUtil.SimpleHttp;
using BPUtil.SimpleHttp.WebSockets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace NetworkSpeedTest
{
	public class WebServer : HttpServer
	{
		private long rnd = StaticRandom.Next(int.MinValue, int.MaxValue);
		private Stopwatch sw = new Stopwatch();
		public WebServer()
		{
			sw.Start();
		}
		public override void handleGETRequest(HttpProcessor p)
		{
			string pageLower = p.Request.Page.ToLower();
			if (p.Request.Page == "nstws_dl" || p.Request.Page == "nstws_bidi")
			{
				// Network Speed Test Web Socket - Download Test
				// or Bidirectional Test.  Both use the same logic since we just receive and disregard any payloads the client sends us.

				// Websocket header length depends on payload length:
				// * For payload length 125 bytes or smaller, header is 6 bytes.
				// * For payload length 126 to 65535, header is 8 bytes.
				// * For payload length 65536 or larger, header is 14 bytes.

				// Here, send websocket frames that are all the same size, and just send them in a loop as quickly as we can.  The Send method will block if the send buffer is full.
				//int packetSize = 62500 - 8;
				//p.GetTcpClient().SendBufferSize = 65535;
				int packetSize = p.Request.GetIntParam("packetSize", 62500);
				packetSize = packetSize.Clamp(125, 1000000);
				if (packetSize <= 125 + 6)
					packetSize -= 6;
				else if (packetSize <= 65535 + 8)
					packetSize -= 8;
				else
					packetSize -= 14;
				p.GetTcpClient().SendBufferSize = Math.Min(65535, packetSize + 100);
				p.GetTcpClient().ReceiveTimeout = 35000;
				byte[] buf = ByteUtil.GenerateRandomBytes(packetSize);
				bool connected = true;
				WebSocket ws = new WebSocket(p, frame => { }, closeFrame => { connected = false; });
				while (connected)
					ws.Send(buf);
			}
			else if (p.Request.Page == "nstws_ul")
			{
				// Network Speed Test Web Socket - Upload Test

				// Here, we just ignore whatever frames the client sends us.
				bool connected = true;
				WebSocket ws = new WebSocket(p, frame => { }, closeFrame => { connected = false; });
				while (connected && p.CheckIfStillConnected())
					Thread.Sleep(10);
			}
			else if (p.Request.Page == "nstws_ping")
			{
				// Network Speed Test Web Socket - Ping Test
				bool connected = true;
				WebSocket ws = null;
				ws = new WebSocket(p, frame =>
				{
					// Whatever the client sends us, just echo it back so they can tell which ping we are responding to.
					if (frame is WebSocketBinaryFrame binFrame)
						ws.Send(binFrame.Data);
					else if (frame is WebSocketTextFrame txtFrame)
						ws.Send(txtFrame.Text);
				}, closeFrame => { connected = false; });
				while (connected && p.CheckIfStillConnected())
					Thread.Sleep(10);
			}
			else if (p.Request.Page == "HEADERS")
			{
				p.Response.FullResponseUTF8(string.Join(Environment.NewLine, p.Request.Headers.Select(h => h.Key + ": " + h.Value)), "text/plain; charset=utf-8");
			}
			else if (p.Request.Page == "IP")
			{
				p.Response.FullResponseUTF8(p.RemoteIPAddressStr, "text/plain; charset=utf-8");
			}
			else
			{
				string path = p.Request.Page;
				if (path == "")
					path = "default.html";

				string wwwPath = Globals.ApplicationDirectoryBase + "www/";
#if DEBUG
				if (System.Diagnostics.Debugger.IsAttached)
					wwwPath = Globals.ApplicationDirectoryBase + "../../www/";
#endif
				DirectoryInfo WWWDirectory = new DirectoryInfo(wwwPath);
				string wwwDirectoryBase = WWWDirectory.FullName.Replace('\\', '/').TrimEnd('/') + '/';
				FileInfo fi = new FileInfo(wwwDirectoryBase + path);
				string targetFilePath = fi.FullName.Replace('\\', '/');
				string vueJsDevPath = wwwDirectoryBase + "../vue.js";
				if (path == "vue.js" && !fi.Exists && File.Exists(vueJsDevPath))
				{
					// Load vue.js from the parent directory if it exists (debug builds within dev environment should hit this).
					fi = new FileInfo(vueJsDevPath);
				}
				else if (!targetFilePath.StartsWith(wwwDirectoryBase) || targetFilePath.Contains("../"))
				{
					p.Response.Simple("400 Bad Request");
					return;
				}
				if (!fi.Exists)
					return;
				if ((fi.Extension == ".html" || fi.Extension == ".htm") && fi.Length < 256000)
				{
					string html = File.ReadAllText(fi.FullName);
					html = html.Replace("%%VERSION%%", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
					html = html.Replace("%%RND%%", rnd.ToString());
#if DEBUG
					if (File.Exists(vueJsDevPath))
						html = html.Replace("%%VUEJS%%", "vue.js");
#endif
					html = html.Replace("%%VUEJS%%", "vue.min.js");

					p.Response.FullResponseUTF8(html, Mime.GetMimeType(fi.Extension));
				}
				else
				{
					StaticFileOptions options = new StaticFileOptions();
					if (pageLower.StartsWith(".well-known/acme-challenge/"))
						options.ContentTypeOverride = "text/plain";
					p.Response.StaticFile(fi.FullName, options);
				}
			}
		}

		protected override void stopServer()
		{
		}

		public override void handlePOSTRequest(HttpProcessor p)
		{
		}
	}
}
