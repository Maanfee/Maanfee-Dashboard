using Maanfee.Logging.Domain;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace Maanfee.Logging.Console
{
    public class LoggingInitializer : IAsyncDisposable
    {
        public LoggingInitializer(HubConnection loggingHubConnection)
        {
            LoggingHubConnection = loggingHubConnection;
        }

        private readonly HubConnection LoggingHubConnection;

        private CancellationTokenSource cts = new CancellationTokenSource();

        public async Task InitializedAsync()
        {
            await ConnectWithRetryAsync(cts.Token);

            LoggingHubConnection.Closed += async (Error) =>
            {
                OnClosed();
                await ConnectWithRetryAsync(cts.Token);
            };
            LoggingHubConnection.Reconnected += async (Message) =>
            {
                OnReconnected();
                await Task.Delay(100);
            };
        }

        public async Task<bool> ConnectWithRetryAsync(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    await LoggingHubConnection.StartAsync(token);
                    return true;
                }
                catch when (token.IsCancellationRequested)
                {
                    return false;
                }
                catch
                {
                    await Task.Delay(1000);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            cts.Cancel();
            cts.Dispose();
            await LoggingHubConnection.DisposeAsync();
        }

        #region - Events -

        public event Action Closed;

        private void OnClosed() => Closed?.Invoke();

        // *****************************************************

        public event Action Reconnected;

        private void OnReconnected() => Reconnected?.Invoke();

        #endregion
    }
}
