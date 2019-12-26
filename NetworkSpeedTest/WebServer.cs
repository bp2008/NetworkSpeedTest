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

namespace NetworkSpeedTest
{
	public class WebServer : HttpServer
	{
		private long rnd = StaticRandom.Next(int.MinValue, int.MaxValue);
		private Stopwatch sw = new Stopwatch();
		NSTWebSocketServer wss;
		public WebServer(int port) : base(port)
		{
			//SendBufferSize = 128;
			//ReceiveBufferSize = 128;
			sw.Start();
			wss = new NSTWebSocketServer(port);
			wss.Start();
		}
		public override void handleGETRequest(HttpProcessor p)
		{
			string pageLower = p.requestedPage.ToLower();
			if (p.requestedPage == "randomdata")
			{
				p.writeSuccess("application/x-binary");
				p.outputStream.Flush();

				int testSec = p.GetIntParam("testsec", 5);
				testSec = BPMath.Clamp(testSec, 1, 30);

				long endTime = sw.ElapsedMilliseconds + (long)TimeSpan.FromSeconds(testSec).TotalMilliseconds;
				byte[] randomData = StaticRandom.NextBytes(p.tcpClient.SendBufferSize);
				while (sw.ElapsedMilliseconds < endTime)
				{
					p.tcpStream.Write(randomData, 0, randomData.Length);
				}
			}
			else if (p.requestedPage == "nstws")
			{
				wss.AcceptIncomingConnection(p.tcpClient);
			}
			else if (p.requestedPage == "HEADERS")
			{
				p.writeSuccess("text/plain");
				p.outputStream.Write(string.Join(Environment.NewLine, p.httpHeadersRaw.Select(h => h.Key + ": " + h.Value)));
			}
			else if (p.requestedPage == "IP")
			{
				p.writeSuccess("text/plain");
				p.outputStream.Write(p.RemoteIPAddressStr);
			}
			else
			{
				if (p.requestedPage == "")
					p.requestedPage = "default.html";

				string wwwPath = Globals.ApplicationDirectoryBase + "www/";
#if DEBUG
				if (System.Diagnostics.Debugger.IsAttached)
					wwwPath = Globals.ApplicationDirectoryBase + "../../www/";
#endif
				DirectoryInfo WWWDirectory = new DirectoryInfo(wwwPath);
				string wwwDirectoryBase = WWWDirectory.FullName.Replace('\\', '/').TrimEnd('/') + '/';
				FileInfo fi = new FileInfo(wwwDirectoryBase + p.requestedPage);
				string targetFilePath = fi.FullName.Replace('\\', '/');
				if (!targetFilePath.StartsWith(wwwDirectoryBase) || targetFilePath.Contains("../"))
				{
					p.writeFailure("400 Bad Request");
					return;
				}
				if (!fi.Exists)
					return;
				if ((fi.Extension == ".html" || fi.Extension == ".htm") && fi.Length < 256000)
				{
					string html = File.ReadAllText(fi.FullName);
					html = html.Replace("%%VERSION%%", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
					html = html.Replace("%%RND%%", rnd.ToString());

					byte[] data = Encoding.UTF8.GetBytes(html);
					p.writeSuccess(Mime.GetMimeType(fi.Extension), data.Length);
					p.outputStream.Flush();
					p.tcpStream.Write(data, 0, data.Length);
					p.tcpStream.Flush();
				}
				else
				{
					string mime = Mime.GetMimeType(fi.Extension);
					if (pageLower.StartsWith(".well-known/acme-challenge/"))
						mime = "text/plain";
					if (fi.LastWriteTimeUtc.ToString("R") == p.GetHeaderValue("if-modified-since"))
					{
						p.writeSuccess(mime, -1, "304 Not Modified");
						return;
					}
					p.writeSuccess(mime, fi.Length, additionalHeaders: GetCacheLastModifiedHeaders(TimeSpan.FromHours(1), fi.LastWriteTimeUtc));
					p.outputStream.Flush();
					using (FileStream fs = fi.OpenRead())
					{
						fs.CopyTo(p.tcpStream);
					}
					p.tcpStream.Flush();
				}
			}
		}
		private List<KeyValuePair<string, string>> GetCacheLastModifiedHeaders(TimeSpan maxAge, DateTime lastModifiedUTC)
		{
			List<KeyValuePair<string, string>> additionalHeaders = new List<KeyValuePair<string, string>>();
			additionalHeaders.Add(new KeyValuePair<string, string>("Cache-Control", "max-age=" + (long)maxAge.TotalSeconds + ", public"));
			additionalHeaders.Add(new KeyValuePair<string, string>("Last-Modified", lastModifiedUTC.ToString("R")));
			return additionalHeaders;
		}

		public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
		{
			string pageLower = p.requestedPage.ToLower();
		}

		protected override void stopServer()
		{
			wss.Stop();
		}
	}
}
