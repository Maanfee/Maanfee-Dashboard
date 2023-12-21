using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Logging.Console;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Maanfee.Dashboard.Services.Controllers.Providers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class WebServiceProvidersController : _BaseController<WebServiceProvidersController>
    {
        public WebServiceProvidersController(_BaseContext_SQLServer context
            , CommonService commonService
            , HttpClient http
            , ILogger<WebServiceProvidersController> logger
            , LoggingInitializer loggingInitializer
            , HubConnection loggingHubConnection
            ) : base(context, commonService, http, logger, loggingInitializer, loggingHubConnection)
        {
        }

        [HttpGet("GetUsers")]
        // GET: api/WebServiceProviders/GetUsers
        public async Task<CallbackResult<IEnumerable<GetUserViewModel>>> GetUsers()
        {
            try
            {
                var users = await CommonService.GetUsers()
                    .Include(x => x.Gender)
                    .Include(x => x.UserDepartments)
                    .Select(x => new GetUserViewModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Password = x.Password,
                        Name = x.Name,
                        Avatar = x.Avatar,
                        FatherName = x.FatherName,
                        UserDepartmentsTitle = string.Join(" , ", x.UserDepartments.Select(x => x.Department.Title)),
                        Gender = x.Gender,
                        Role = db_SQLServer.AspNetRoles.FirstOrDefault(a => a.Id == (db_SQLServer.AspNetUserRoles.FirstOrDefault(z => z.UserId == x.Id)).RoleId),
                        NationalCode = x.NationalCode,
                        PhoneNumber = x.PhoneNumber,
                        PersonalCode = x.PersonalCode,
                        RoleName = db_SQLServer.AspNetRoles.FirstOrDefault(a => a.Id == (db_SQLServer.AspNetUserRoles.FirstOrDefault(z => z.UserId == x.Id)).RoleId).Name ?? "-",
                        UserName = x.UserName,
                    }).ToListAsync();

                if (users == null)
                {
                    return new CallbackResult<IEnumerable<GetUserViewModel>>(null, new ExceptionError(DashboardResource.MessageChangeIsNotPossible));
                }

                return new CallbackResult<IEnumerable<GetUserViewModel>>(users, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<GetUserViewModel>>(null, new ExceptionError(ex.Message));
            }
        }

    }
}
