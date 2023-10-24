using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Logging.Console;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Net.Http;

namespace Maanfee.Dashboard.Services.Controllers
{
    public class _BaseController : ControllerBase
    {
        public _BaseController(_BaseContext_SQLServer context, CommonService commonService, HttpClient http, IHubContext<LoggingHub> _LoggingHub)
        {
            db_SQLServer = context;
            CommonService = commonService;
            Http = http;
            LoggingHub = _LoggingHub;
        }

        protected readonly _BaseContext_SQLServer db_SQLServer;

        protected readonly CommonService CommonService;

        protected HttpClient Http;

        protected readonly IHubContext<LoggingHub> LoggingHub;

        protected CurrentUser GetCurrentUserInfo()
        {
            return new CurrentUser
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Claims = User.Claims.ToDictionary(c => c.Type, c => c.Value),
                Id = User.Claims.FirstOrDefault(x => x.Type == "Id").Value,
                Name = User.Claims.FirstOrDefault(x => x.Type == "Name").Value,
            };
        }
    }
}
