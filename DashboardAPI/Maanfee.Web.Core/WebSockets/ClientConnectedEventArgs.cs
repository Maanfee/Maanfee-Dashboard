using System;
using System.Net;

namespace Maanfee.Web.Core.WebSockets
{
	public class ClientConnectedEventArgs : EventArgs
	{
		public string IpPort { get; }

		public HttpListenerRequest HttpRequest { get; }

		internal ClientConnectedEventArgs(string ipPort, HttpListenerRequest http)
		{
			IpPort = ipPort;
			HttpRequest = http;
		}
	}
}
