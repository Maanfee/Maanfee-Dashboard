using System;

namespace Maanfee.Web.Core.WebSockets
{
	public class ClientDisconnectedEventArgs : EventArgs
	{
		public string IpPort { get; }

		internal ClientDisconnectedEventArgs(string ipPort)
		{
			IpPort = ipPort;
		}
	}
}
