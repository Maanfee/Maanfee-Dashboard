using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maanfee.Web.Core.WebSockets
{
    public class MaanfeeWsClient : IDisposable
    {
        public Action<string> Logger;

        private string _Header = "[MaanfeeWsClient] ";

        private bool _AcceptInvalidCertificates = true;

        private Uri _ServerUri;

        private string _ServerIp;

        private int _ServerPort;

        private string _ServerIpPort;

        private string _Url;

        private ClientWebSocket _ClientWs;

        private readonly SemaphoreSlim _SendLock = new SemaphoreSlim(1);

        private CancellationTokenSource _TokenSource = new CancellationTokenSource();

        private CancellationToken _Token;

        private Statistics _Stats = new Statistics();

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

        public bool Connected
        {
            get
            {
                if (_ClientWs != null && _ClientWs.State == WebSocketState.Open)
                {
                    return true;
                }
                return false;
            }
        }

        public bool EnableStatistics { get; set; } = true;


        public Statistics Stats => _Stats;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public event EventHandler ServerConnected;

        public event EventHandler ServerDisconnected;

        public MaanfeeWsClient(string serverIp, int serverPort, bool ssl)
        {
            if (string.IsNullOrEmpty(serverIp))
            {
                throw new ArgumentNullException("serverIp");
            }
            if (serverPort < 1)
            {
                throw new ArgumentOutOfRangeException("serverPort");
            }
            _ServerIp = serverIp;
            _ServerPort = serverPort;
            _ServerIpPort = serverIp + ":" + serverPort;
            if (ssl)
            {
                _Url = "wss://" + _ServerIp + ":" + _ServerPort;
            }
            else
            {
                _Url = "ws://" + _ServerIp + ":" + _ServerPort;
            }
            _ServerUri = new Uri(_Url);
            _Token = _TokenSource.Token;
            _ClientWs = new ClientWebSocket();
        }

        public MaanfeeWsClient(Uri uri)
        {
            _ServerUri = uri;
            _ServerIp = uri.Host;
            _ServerPort = uri.Port;
            _ServerIpPort = uri.Host + ":" + uri.Port;
            _Token = _TokenSource.Token;
            _ClientWs = new ClientWebSocket();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            _Stats = new Statistics();
            if (_AcceptInvalidCertificates)
            {
                ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
            }
            _ClientWs.ConnectAsync(_ServerUri, _Token).ContinueWith(new Action<Task>(AfterConnect));
        }

        public Task StartAsync()
        {
            _Stats = new Statistics();
            if (_AcceptInvalidCertificates)
            {
                ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
            }
            return _ClientWs.ConnectAsync(_ServerUri, _Token).ContinueWith(new Action<Task>(AfterConnect));
        }

        public void Stop()
        {
            _ClientWs.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).Wait();
        }

        public async Task<bool> SendAsync(string data, CancellationToken token = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }
            return await MessageWriteAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, token);
        }

        public async Task<bool> SendAsync(byte[] data, CancellationToken token = default(CancellationToken))
        {
            if (data == null || data.Length < 1)
            {
                throw new ArgumentNullException("data");
            }
            return await MessageWriteAsync(data, WebSocketMessageType.Binary, token);
        }

        public async Task<bool> SendAsync(byte[] data, WebSocketMessageType msgType, CancellationToken token = default(CancellationToken))
        {
            if (data == null || data.Length < 1)
            {
                throw new ArgumentNullException("data");
            }
            return await MessageWriteAsync(data, msgType, token);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_ClientWs != null && _ClientWs.State == WebSocketState.Open)
                {
                    Stop();
                    _ClientWs.Dispose();
                }
                _TokenSource.Cancel();
                Logger?.Invoke(_Header + "dispose complete");
            }
        }

        private void AfterConnect(Task task)
        {
            if (task.IsCompleted)
            {
                if (_ClientWs.State == WebSocketState.Open)
                {
                    Task.Run(delegate
                    {
                        Task.Run(() => DataReceiver(), _Token);
                        this.ServerConnected?.Invoke(this, EventArgs.Empty);
                    }, _Token);
                }
                else
                {
                    this.ServerDisconnected?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                this.ServerDisconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        private async Task DataReceiver()
        {
            try
            {
                while (!_Token.IsCancellationRequested)
                {
                    MessageReceivedEventArgs msg = await MessageReadAsync();
                    if (msg != null)
                    {
                        if (EnableStatistics)
                        {
                            _Stats.IncrementReceivedMessages();
                            _Stats.AddReceivedBytes(msg.Data.Length);
                        }
                        await Task.Run(delegate
                          {
                              this.MessageReceived?.Invoke(this, msg);
                          }, _Token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Logger?.Invoke(_Header + "data receiver canceled");
            }
            catch (WebSocketException)
            {
                Logger?.Invoke(_Header + "websocket disconnected");
            }
            catch (Exception ex3)
            {
                Logger?.Invoke(_Header + "exception: " + Environment.NewLine + ex3.ToString());
            }
            this.ServerDisconnected?.Invoke(this, EventArgs.Empty);
        }

        private async Task<MessageReceivedEventArgs> MessageReadAsync()
        {
            if (_ClientWs == null)
            {
                return null;
            }
            byte[] buffer2 = new byte[65536];
            byte[] data = null;
            WebSocketReceiveResult result = null;
            using (MemoryStream dataMs = new MemoryStream())
            {
                buffer2 = new byte[buffer2.Length];
                ArraySegment<byte> bufferSegment = new ArraySegment<byte>(buffer2);
                if (_ClientWs.State == WebSocketState.CloseReceived || _ClientWs.State == WebSocketState.Closed)
                {
                    throw new WebSocketException("Websocket close received");
                }
                while (_ClientWs.State == WebSocketState.Open)
                {
                    result = await _ClientWs.ReceiveAsync(bufferSegment, _Token);
                    if (result.Count > 0)
                    {
                        await dataMs.WriteAsync(buffer2, 0, result.Count);
                    }
                    if (result.EndOfMessage)
                    {
                        data = dataMs.ToArray();
                        break;
                    }
                }
            }
            return new MessageReceivedEventArgs(_ServerIpPort, data, result.MessageType);
        }

        private async Task<bool> MessageWriteAsync(byte[] data, WebSocketMessageType msgType, CancellationToken token)
        {
            bool disconnectDetected = false;
            using (CancellationTokenSource.CreateLinkedTokenSource(_Token, token))
            {
                _ = 1;
                try
                {
                    if (_ClientWs == null || _ClientWs.State != WebSocketState.Open)
                    {
                        Logger?.Invoke(_Header + "not connected");
                        disconnectDetected = true;
                        return false;
                    }
                    await _SendLock.WaitAsync(_Token);
                    try
                    {
                        await _ClientWs.SendAsync(new ArraySegment<byte>(data, 0, data.Length), msgType, endOfMessage: true, token);
                    }
                    finally
                    {
                        _SendLock.Release();
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
                        Logger?.Invoke(_Header + "canceled");
                        disconnectDetected = true;
                    }
                    else if (token.IsCancellationRequested)
                    {
                        Logger?.Invoke(_Header + "message send canceled");
                    }
                    return false;
                }
                catch (OperationCanceledException)
                {
                    if (_Token.IsCancellationRequested)
                    {
                        Logger?.Invoke(_Header + "canceled");
                        disconnectDetected = true;
                    }
                    else if (token.IsCancellationRequested)
                    {
                        Logger?.Invoke(_Header + "message send canceled");
                    }
                    return false;
                }
                catch (WebSocketException)
                {
                    Logger?.Invoke(_Header + "websocket disconnected");
                    disconnectDetected = true;
                    return false;
                }
                catch (ObjectDisposedException)
                {
                    Logger?.Invoke(_Header + "disposed");
                    disconnectDetected = true;
                    return false;
                }
                catch (SocketException)
                {
                    Logger?.Invoke(_Header + "socket disconnected");
                    disconnectDetected = true;
                    return false;
                }
                catch (InvalidOperationException)
                {
                    Logger?.Invoke(_Header + "disconnected due to invalid operation");
                    disconnectDetected = true;
                    return false;
                }
                catch (IOException)
                {
                    Logger?.Invoke(_Header + "IO disconnected");
                    disconnectDetected = true;
                    return false;
                }
                catch (Exception ex8)
                {
                    Logger?.Invoke(_Header + "exception: " + Environment.NewLine + ex8.ToString());
                    disconnectDetected = true;
                    return false;
                }
                finally
                {
                    if (disconnectDetected)
                    {
                        Dispose();
                        this.ServerDisconnected?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
