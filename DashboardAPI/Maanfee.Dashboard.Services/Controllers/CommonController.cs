using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Logging.Console;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Maanfee.Dashboard.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CommonController : _BaseController<CommonController>
    {
        public CommonController(_BaseContext_SQLServer context
            , CommonService commonService
            , HttpClient http
            , ILogger<CommonController> logger
            , LoggingInitializer loggingInitializer
            , HubConnection loggingHubConnection
            ) : base(context, commonService, http, logger, loggingInitializer, loggingHubConnection)
        {
        }

    }
}
