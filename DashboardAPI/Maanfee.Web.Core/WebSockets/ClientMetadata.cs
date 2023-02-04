using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;

namespace Maanfee.Web.Core.WebSockets
{
	internal class ClientMetadata
	{
		internal string Ip;

		internal int Port;

		internal HttpListenerContext HttpContext;

		internal WebSocket Ws;

		internal WebSocketContext WsContext;

		internal readonly CancellationTokenSource TokenSource;

		internal readonly SemaphoreSlim SendLock = new SemaphoreSlim(1);

		internal string IpPort => Ip + ":" + Port;

		internal ClientMetadata(HttpListenerContext httpContext, WebSocket ws, WebSocketContext wsContext, CancellationTokenSource tokenSource)
		{
			HttpContext = httpContext ?? throw new ArgumentNullException("httpContext");
			Ws = ws ?? throw new ArgumentNullException("ws");
			WsContext = wsContext ?? throw new ArgumentNullException("wsContext");
			TokenSource = tokenSource ?? throw new ArgumentNullException("tokenSource");
			Ip = HttpContext.Request.RemoteEndPoint.Address.ToString();
			Port = HttpContext.Request.RemoteEndPoint.Port;
		}
	}
}
