using Maanfee.Web.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Maanfee.Dashboard.Services.Controllers.SystemConfigurations
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AppSettingsController : ControllerBase
    {
        public AppSettingsController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }

        //[HttpGet("GetApplicationConfiguration")]
        //GET: /api/AppSettings/GetApplicationConfiguration
        //public CallbackResult<SettingViewModel> GetApplicationConfiguration()
        //{
        //    try
        //    { 
        //        var Settings = new SettingViewModel
        //        {
        //            ApplicationName = Configuration.GetValue<string>("Settings:ApplicationName"),
        //            Version = Configuration.GetValue<string>("Settings:Version"),
        //        };

        //        return new CallbackResult<SettingViewModel>(Settings, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new CallbackResult<SettingViewModel>(null, new ExceptionError(ex.Message));
        //    }
        //}

        [HttpGet("GetTest")]
        //GET: /api/AppSettings/GetTest
        public CallbackResult<string> GetTest()
        {
            try
            {
                string dt = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

                return new CallbackResult<string>(dt, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<string>(null, new ExceptionError(ex.Message));
            }
        }

    }
}
