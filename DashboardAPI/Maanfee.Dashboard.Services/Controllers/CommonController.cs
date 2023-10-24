using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Logging.Console;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http;

namespace Maanfee.Dashboard.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CommonController : _BaseController
    {
        public CommonController(_BaseContext_SQLServer context, CommonService CommonService, HttpClient http, IHubContext<LoggingHub> loggingHub) : base(context, CommonService, http, loggingHub)
        {

        }
      
    }
}
