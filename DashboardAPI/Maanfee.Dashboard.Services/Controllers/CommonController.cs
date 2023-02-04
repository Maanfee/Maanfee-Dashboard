using Maanfee.Dashboard.Domain.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Maanfee.Dashboard.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CommonController : _BaseController
    {
        public CommonController(_BaseContext_SQLServer context, CommonService CommonService, HttpClient http) : base(context, CommonService, http)
        {

        }
      
    }
}
