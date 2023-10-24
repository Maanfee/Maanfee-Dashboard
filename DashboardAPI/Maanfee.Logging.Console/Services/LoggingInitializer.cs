using Microsoft.AspNetCore.SignalR.Client;

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

            LoggingHubConnection.Closed += error =>
            {
                return ConnectWithRetryAsync(cts.Token);
            };
        }

        private async Task<bool> ConnectWithRetryAsync(CancellationToken token)
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
                    await Task.Delay(5000);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            cts.Cancel();
            cts.Dispose();
            await LoggingHubConnection.DisposeAsync();
        }
    }
}
