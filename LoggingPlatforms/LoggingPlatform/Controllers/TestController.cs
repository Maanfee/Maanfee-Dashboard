using Maanfee.Logging.Console;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;

            hubConnection = new HubConnectionBuilder()
              .WithUrl("http://localhost:22001/LoggingHub")
              .Build();

            Task.Run(async () =>
            {
                await hubConnection.StartAsync();
            });
        }

        private readonly ILogger<TestController> _logger;

        private HubConnection hubConnection;

        [Authorize]
        [HttpGet("GetStatus")]
        public string GetStatus()
        {
            return "Authorized";
        }

        [HttpGet("ConnectionTest")]
        public async Task<string> ConnectionTest()
        {
            if (hubConnection != null)
            {
                await hubConnection.SendAsync("SendMessageAsync", new LogInfo
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