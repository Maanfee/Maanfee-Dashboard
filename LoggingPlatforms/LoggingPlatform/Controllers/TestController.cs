using Maanfee.Logging.Console;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LoggingPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController(ILogger<TestController> logger, LoggingInitializer loggingInitializer, HubConnection loggingHubConnection)
        {
            _logger = logger;

            try
            {
                LoggingInitializer = loggingInitializer;
                LoggingHubConnection = loggingHubConnection;

                //LoggingHubConnection = new HubConnectionBuilder()
                //  .WithUrl("http://localhost:22001/LoggingHub")
                //  .Build();

                Task.Run(async () =>
                {
                    //await LoggingHubConnection.StartAsync();
                    await LoggingInitializer.InitializedAsync();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private readonly ILogger<TestController> _logger;

        protected HubConnection LoggingHubConnection;

        protected LoggingInitializer LoggingInitializer;

        [Authorize]
        [HttpGet("GetStatus")]
        public string GetStatus()
        {
            return "Authorized";
        }

        [HttpGet("ConnectionTest")]
        public async Task<string> ConnectionTest()
        {
            await Task.Delay(1000);

            if (LoggingHubConnection != null)
            {
                await LoggingHubConnection.SendAsync("SendMessageAsync", new LogInfo
                {
                    Platform = LoggingPlatformDefaultValue.System,
                    Message = "Web API ...",
                    LogDate = DateTime.Now,
                    Level = Maanfee.Logging.Console.LogLevel.Warning,
                });
            }

            return "The connection is established.";
        }

    }

}