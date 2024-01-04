using Maanfee.Logging.Console;
using Maanfee.Logging.Domain;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace Maanfee.Logging.Server.Services
{
    public sealed class LogStoreService : BackgroundService
    {
        public LogStoreService(HttpClient http
            , LoggingInitializer loggingInitializer
            , HubConnection loggingHubConnection
            , ILogger<LogStoreService> logger)
        {
            try
            {
                Http = http;
                LoggingInitializer = loggingInitializer;
                LoggingHubConnection = loggingHubConnection;
                Logger = logger;

                Task.Run(async () =>
                {
                    await LoggingInitializer.InitializedAsync();
                });
            }
            catch
            {

            }
        }

        private HubConnection LoggingHubConnection;

        private HttpClient Http;

        private LoggingInitializer LoggingInitializer;

        private readonly ILogger<LogStoreService> Logger;

        private Uri uri => new Uri($"http://localhost:4031/api/Logging/Create");

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            LoggingHubConnection.On<LogInfo>("ReceiveMessage", async (Log) =>
            {
                try
                {
                    Logger.LogInformation($"{Log.Message} : {Log.LogDate.ToShortDateString()}");

                    var PostResult = await Http.PostAsJsonAsync(uri,
                            new SubmitLogInfo
                            {
                                IdLoggingLevel = Log.IdLoggingLevel,
                                IdLoggingPlatform = Log.IdLoggingPlatform,
                                LogDate = Log.LogDate,
                                Message = Log.Message,
                            });

                    if (PostResult.IsSuccessStatusCode)
                    {
                        var JsonResult = await PostResult.Content.ReadFromJsonAsync<CallbackResult<LogInfo>>();
                        if (JsonResult.Data != null)
                        {
                            // Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageSavedSuccessfully, Severity.Success);
                            // MudDialog.Close(DialogResult.Ok(SubmitGroupViewModel));
                        }
                        else
                        {
                            // Snackbar.Add(MessageHandler.ErrorHandler(JsonResult.Error), Severity.Error);
                        }
                    }
                    else
                    {
                        // Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
                        Logger.LogInformation(PostResult.Content.ReadAsStringAsync().Result);
                    }
                }
                catch (Exception ex)
                {
                    await LoggingHubConnection.SendAsync("SendMessageAsync", new LogInfo
                    {
                        IdLoggingPlatform = LoggingPlatformDefaultValue.System,
                        Message = "Save logging error ..." + ex.InnerException,
                        LogDate = DateTime.Now,
                        IdLoggingLevel = LoggingLevelDefaultValue.Error,
                    });
                }
            });

            await Task.Delay(100, stoppingToken);
        }
    }
}
