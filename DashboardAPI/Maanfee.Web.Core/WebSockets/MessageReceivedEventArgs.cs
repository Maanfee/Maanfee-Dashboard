using System;
using System.Net.WebSockets;

namespace Maanfee.Web.Core.WebSockets
{
	public class MessageReceivedEventArgs : EventArgs
	{
		public WebSocketMessageType MessageType = WebSocketMessageType.Binary;

		public string IpPort { get; }

		public byte[] Data { get; }

		internal MessageReceivedEventArgs(string ipPort, byte[] data, WebSocketMessageType messageType)
		{
			IpPort = ipPort;
			Data = data;
			MessageType = messageType;
		}
	}
}
