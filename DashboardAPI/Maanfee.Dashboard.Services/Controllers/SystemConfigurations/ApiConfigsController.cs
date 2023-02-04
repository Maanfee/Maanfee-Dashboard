using Maanfee.Dashboard.Domain.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maanfee.Dashboard.Services.Controllers.SystemConfigurations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class ApiConfigsController : ControllerBase
    {
        public ApiConfigsController(_BaseContext_SQLite context, CommonService commonService)
        {
            db_SQLite = context;
            CommonService = commonService;
        }

        protected readonly _BaseContext_SQLite db_SQLite;
        protected readonly CommonService CommonService;

        //return new CallbackResult<ApiConfig>(null, new ExceptionError("KOOOOOOONI"));
    }
}
