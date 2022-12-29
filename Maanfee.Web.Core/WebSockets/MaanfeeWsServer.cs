using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maanfee.Web.Core.WebSockets
{
	public class MaanfeeWsServer : IDisposable
	{
		public List<string> PermittedIpAddresses = new List<string>();

		public Action<string> Logger;

		public Action<HttpListenerContext> HttpHandler;

		private string _Header = "[MaanfeeWsServer] ";

		private bool _AcceptInvalidCertificates = true;

		private string _ListenerPrefix;

		private HttpListener _Listener;

		private readonly object _PermittedIpsLock = new object();

		private ConcurrentDictionary<string, ClientMetadata> _Clients;

		private CancellationTokenSource _TokenSource;

		private CancellationToken _Token;

		private Task _AcceptConnectionsTask;

		private Statistics _Stats = new Statistics();

		public bool IsListening
		{
			get
			{
				if (_Listener != null)
				{
					return _Listener.IsListening;
				}
				return false;
			}
		}

		public bool EnableStatistics { get; set; } = true;


		public bool AcceptInvalidCertificates
		{
			get
			{
				return _AcceptInvalidCertificates;
			}
			set
			{
				_AcceptInvalidCertificates = value;
			}
		}

		public Statistics Stats => _Stats;

		public event EventHandler<ClientConnectedEventArgs> ClientConnected;

		public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

		public event EventHandler ServerStopped;

		public event EventHandler<MessageReceivedEventArgs> MessageReceived;

		public MaanfeeWsServer(string listenerHostname = "localhost", int listenerPort = 9000, bool ssl = false)
		{
			if (listenerPort < 0)
			{
				throw new ArgumentOutOfRangeException("listenerPort");
			}
			if (string.IsNullOrEmpty(listenerHostname))
			{
				listenerHostname = "localhost";
			}
			if (ssl)
			{
				_ListenerPrefix = "https://" + listenerHostname + ":" + listenerPort + "/";
			}
			else
			{
				_ListenerPrefix = "http://" + listenerHostname + ":" + listenerPort + "/";
			}
			_Listener = new HttpListener();
			_Listener.Prefixes.Add(_ListenerPrefix);
			_TokenSource = new CancellationTokenSource();
			_Token = _TokenSource.Token;
			_Clients = new ConcurrentDictionary<string, ClientMetadata>();
		}

		public MaanfeeWsServer(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (uri.Port < 0)
			{
				throw new ArgumentException("Port must be zero or greater.");
			}
			string host;
			if (!IPAddress.TryParse(uri.Host, out var _))
			{
				IPHostEntry hostEntry = Dns.GetHostEntry(uri.Host);
				if (hostEntry.AddressList.Length == 0)
				{
					throw new ArgumentException("Cannot resolve address to IP.");
				}
				host = hostEntry.AddressList.First().ToString();
			}
			else
			{
				host = uri.Host;
			}
			UriBuilder uriBuilder = new UriBuilder(uri)
			{
				Host = host
			};
			_ListenerPrefix = uriBuilder.ToString();
			_Listener = new HttpListener();
			_Listener.Prefixes.Add(_ListenerPrefix);
			_TokenSource = new CancellationTokenSource();
			_Token = _TokenSource.Token;
			_Clients = new ConcurrentDictionary<string, ClientMetadata>();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}

		public void Start()
		{
			if (IsListening)
			{
				throw new InvalidOperationException("Maanfee websocket server is already running.");
			}
			_Stats = new Statistics();
			Logger?.Invoke(_Header + "starting " + _ListenerPrefix);
			if (_AcceptInvalidCertificates)
			{
				ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
			}
			_TokenSource = new CancellationTokenSource();
			_Token = _TokenSource.Token;
			_Listener.Start();
			_AcceptConnectionsTask = Task.Run(() => AcceptConnections(_Token), _Token);
		}

		public Task StartAsync(CancellationToken token = default(CancellationToken))
		{
			if (IsListening)
			{
				throw new InvalidOperationException("Maanfee websocket server is already running.");
			}
			_Stats = new Statistics();
			Logger?.Invoke(_Header + "starting " + _ListenerPrefix);
			if (_AcceptInvalidCertificates)
			{
				ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
			}
			_TokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
			_Token = token;
			_Listener.Start();
			_AcceptConnectionsTask = Task.Run(() => AcceptConnections(_Token), _Token);
			return Task.Delay(1);
		}

		public void Stop()
		{
			if (!IsListening)
			{
				throw new InvalidOperationException("Maanfee websocket server is not running.");
			}
			Logger?.Invoke(_Header + "stopping " + _ListenerPrefix);
			_Listener.Stop();
		}

		public Task<bool> SendAsync(string ipPort, string data, CancellationToken token = default(CancellationToken))
		{
			if (string.IsNullOrEmpty(data))
			{
				throw new ArgumentNullException("data");
			}
			if (!_Clients.TryGetValue(ipPort, out var value))
			{
				Logger?.Invoke(_Header + "unable to find client " + ipPort);
				return Task.FromResult(result: false);
			}
			Task<bool> result = MessageWriteAsync(value, Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, token);
			data = null;
			value = null;
			return result;
		}

		public Task<bool> SendAsync(string ipPort, byte[] data, CancellationToken token = default(CancellationToken))
		{
			if (data == null || data.Length < 1)
			{
				throw new ArgumentNullException("data");
			}
			if (!_Clients.TryGetValue(ipPort, out var value))
			{
				Logger?.Invoke(_Header + "unable to find client " + ipPort);
				return Task.FromResult(result: false);
			}
			Task<bool> result = MessageWriteAsync(value, data, WebSocketMessageType.Binary, token);
			value = null;
			data = null;
			return result;
		}

		public Task<bool> SendAsync(string ipPort, byte[] data, WebSocketMessageType msgType, CancellationToken token = default(CancellationToken))
		{
			if (data == null || data.Length < 1)
			{
				throw new ArgumentNullException("data");
			}
			if (!_Clients.TryGetValue(ipPort, out var value))
			{
				Logger?.Invoke(_Header + "unable to find client " + ipPort);
				return Task.FromResult(result: false);
			}
			Task<bool> result = MessageWriteAsync(value, data, msgType, token);
			value = null;
			data = null;
			return result;
		}

		public bool IsClientConnected(string ipPort)
		{
			ClientMetadata value;
			return _Clients.TryGetValue(ipPort, out value);
		}

		public IEnumerable<string> ListClients()
		{
			return _Clients.Keys.ToArray();
		}

		public void DisconnectClient(string ipPort)
		{
			if (_Clients.TryGetValue(ipPort, out var value))
			{
				value.Ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", value.TokenSource.Token).Wait();
				value.TokenSource.Cancel();
				value.Ws.Dispose();
			}
		}

		public TaskAwaiter GetAwaiter()
		{
			return _AcceptConnectionsTask.GetAwaiter();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			if (_Clients != null)
			{
				foreach (KeyValuePair<string, ClientMetadata> client in _Clients)
				{
					client.Value.Ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", client.Value.TokenSource.Token);
					client.Value.TokenSource.Cancel();
				}
			}
			if (_Listener != null)
			{
				if (_Listener.IsListening)
				{
					_Listener.Stop();
				}
				_Listener.Close();
			}
			_TokenSource.Cancel();
		}

		private async Task AcceptConnections(CancellationToken cancelToken)
		{
			_ = 1;
			try
			{
				while (!cancelToken.IsCancellationRequested)
				{
					if (!_Listener.IsListening)
					{
						Task.Delay(100).Wait();
						continue;
					}
					HttpListenerContext ctx = await _Listener.GetContextAsync().ConfigureAwait(continueOnCapturedContext: false);
					string text = ctx.Request.RemoteEndPoint.Address.ToString();
					string ipPort = text + ":" + ctx.Request.RemoteEndPoint.Port;
					lock (_PermittedIpsLock)
					{
						if (PermittedIpAddresses != null && PermittedIpAddresses.Count > 0 && !PermittedIpAddresses.Contains(text))
						{
							Logger?.Invoke(_Header + "rejecting " + ipPort + " (not permitted)");
							ctx.Response.StatusCode = 401;
							ctx.Response.Close();
							continue;
						}
					}
					if (!ctx.Request.IsWebSocketRequest)
					{
						if (HttpHandler == null)
						{
							Logger?.Invoke(_Header + "non-websocket request rejected from " + ipPort);
							ctx.Response.StatusCode = 400;
							ctx.Response.Close();
							continue;
						}
						Logger?.Invoke(_Header + "non-websocket request from " + ipPort + " HTTP-forwarded: " + ctx.Request.HttpMethod.ToString() + " " + ctx.Request.RawUrl);
						HttpHandler(ctx);
						continue;
					}
					await Task.Run(delegate
					{
						Logger?.Invoke(_Header + "starting data receiver for " + ipPort);
						CancellationTokenSource tokenSource = new CancellationTokenSource();
						CancellationToken token = tokenSource.Token;
						Task.Run(async delegate
						{
							WebSocketContext webSocketContext = await ctx.AcceptWebSocketAsync(null);
							WebSocket webSocket = webSocketContext.WebSocket;
							ClientMetadata md = new ClientMetadata(ctx, webSocket, webSocketContext, tokenSource);
							_Clients.TryAdd(md.IpPort, md);
							this.ClientConnected?.Invoke(this, new ClientConnectedEventArgs(md.IpPort, ctx.Request));
							await Task.Run(() => DataReceiver(md), token);
						}, token);
					}, _Token).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (TaskCanceledException)
			{
			}
			catch (OperationCanceledException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			catch (Exception ex4)
			{
				Logger?.Invoke(_Header + "listener exception:" + Environment.NewLine + ex4.ToString());
			}
			finally
			{
				this.ServerStopped?.Invoke(this, EventArgs.Empty);
			}
		}

		private async Task DataReceiver(ClientMetadata md)
		{
			string header = "[MaanfeeWsServer " + md.IpPort + "] ";
			Logger?.Invoke(header + "starting data receiver");
			try
			{
				while (true)
				{
					MessageReceivedEventArgs msg = await MessageReadAsync(md).ConfigureAwait(continueOnCapturedContext: false);
					if (msg == null)
					{
						continue;
					}
					if (EnableStatistics)
					{
						_Stats.IncrementReceivedMessages();
						_Stats.AddReceivedBytes(msg.Data.Length);
					}
					if (msg.Data != null)
					{
						await Task.Run(delegate
						{
							this.MessageReceived?.Invoke(this, msg);
						}, md.TokenSource.Token);
					}
					else
					{
						await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
			}
			catch (TaskCanceledException)
			{
			}
			catch (OperationCanceledException)
			{
			}
			catch (WebSocketException)
			{
			}
			catch (Exception ex4)
			{
				Logger?.Invoke(header + "exception: " + Environment.NewLine + ex4.ToString());
			}
			finally
			{
				string ipPort = md.IpPort;
				this.ClientDisconnected?.Invoke(this, new ClientDisconnectedEventArgs(md.IpPort));
				md.Ws.Dispose();
				Logger?.Invoke(header + "disconnected");
				_Clients.TryRemove(ipPort, out var _);
			}
		}

		private async Task<MessageReceivedEventArgs> MessageReadAsync(ClientMetadata md)
		{
			string header = "[MaanfeeWsServer " + md.IpPort + "] ";
			using MemoryStream ms = new MemoryStream();
			byte[] buffer = new byte[65536];
			ArraySegment<byte> seg = new ArraySegment<byte>(buffer);
			WebSocketReceiveResult webSocketReceiveResult;
			do
			{
				webSocketReceiveResult = await md.Ws.ReceiveAsync(seg, md.TokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
				if (webSocketReceiveResult.CloseStatus.HasValue)
				{
					Logger?.Invoke(header + "close received");
					await md.Ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
					throw new WebSocketException("Websocket closed.");
				}
				if (md.Ws.State != WebSocketState.Open)
				{
					Logger?.Invoke(header + "websocket no longer open");
					throw new WebSocketException("Websocket closed.");
				}
				if (md.TokenSource.Token.IsCancellationRequested)
				{
					Logger?.Invoke(header + "cancel requested");
				}
				if (webSocketReceiveResult.Count > 0)
				{
					ms.Write(buffer, 0, webSocketReceiveResult.Count);
				}
			}
			while (!webSocketReceiveResult.EndOfMessage);
			return new MessageReceivedEventArgs(md.IpPort, ms.ToArray(), webSocketReceiveResult.MessageType);
		}

		private async Task<bool> MessageWriteAsync(ClientMetadata md, byte[] data, WebSocketMessageType msgType, CancellationToken token)
		{
			string header = "[MaanfeeWsServer " + md.IpPort + "] ";
			using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_Token, token, md.TokenSource.Token))
			{
				_ = 1;
				try
				{
					await md.SendLock.WaitAsync(md.TokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
					try
					{
						await md.Ws.SendAsync(new ArraySegment<byte>(data, 0, data.Length), msgType, endOfMessage: true, linkedCts.Token).ConfigureAwait(continueOnCapturedContext: false);
					}
					finally
					{
						md.SendLock.Release();
					}
					if (EnableStatistics)
					{
						_Stats.IncrementSentMessages();
						_Stats.AddSentBytes(data.Length);
					}
					return true;
				}
				catch (TaskCanceledException)
				{
					if (_Token.IsCancellationRequested)
					{
						Logger?.Invoke(header + "server canceled");
					}
					else if (token.IsCancellationRequested)
					{
						Logger?.Invoke(header + "message send canceled");
					}
					else if (md.TokenSource.Token.IsCancellationRequested)
					{
						Logger?.Invoke(header + "client canceled");
					}
				}
				catch (OperationCanceledException)
				{
					if (_Token.IsCancellationRequested)
					{
						Logger?.Invoke(header + "canceled");
					}
					else if (token.IsCancellationRequested)
					{
						Logger?.Invoke(header + "message send canceled");
					}
					else if (md.TokenSource.Token.IsCancellationRequested)
					{
						Logger?.Invoke(header + "client canceled");
					}
				}
				catch (ObjectDisposedException)
				{
					Logger?.Invoke(header + "disposed");
				}
				catch (WebSocketException)
				{
					Logger?.Invoke(header + "websocket disconnected");
				}
				catch (SocketException)
				{
					Logger?.Invoke(header + "socket disconnected");
				}
				catch (InvalidOperationException)
				{
					Logger?.Invoke(header + "disconnected due to invalid operation");
				}
				catch (IOException)
				{
					Logger?.Invoke(header + "IO disconnected");
				}
				catch (Exception ex8)
				{
					Logger?.Invoke(header + "exception: " + Environment.NewLine + ex8.ToString());
				}
				finally
				{
					md = null;
					data = null;
				}
			}
			return false;
		}
	}
}
