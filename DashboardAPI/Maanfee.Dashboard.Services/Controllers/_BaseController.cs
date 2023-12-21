using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Logging.Console;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Maanfee.Dashboard.Services.Controllers
{
    public class _BaseController<T> : ControllerBase
    {
        public _BaseController(_BaseContext_SQLServer context
            , CommonService commonService
            , HttpClient http
            , ILogger<T> logger
            , LoggingInitializer loggingInitializer
            , HubConnection loggingHubConnection)
        {
            try
            {
                db_SQLServer = context;
                Http = http;
                CommonService = commonService;
                Logger = logger;
                LoggingInitializer = loggingInitializer;
                LoggingHubConnection = loggingHubConnection;

                Task.Run(async () =>
                {
                    await LoggingInitializer.InitializedAsync();
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
        }

        protected readonly _BaseContext_SQLServer db_SQLServer;

        protected readonly CommonService CommonService;

        protected HttpClient Http;

        protected readonly ILogger<T> Logger;

        protected HubConnection LoggingHubConnection;

        protected LoggingInitializer LoggingInitializer;

        // ********************************************************
    }
}
