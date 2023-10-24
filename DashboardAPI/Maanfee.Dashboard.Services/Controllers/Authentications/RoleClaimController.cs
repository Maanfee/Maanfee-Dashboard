using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Logging.Console;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Services.Controllers.Authentications
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RoleClaimController : _BaseController
    {
        public RoleClaimController(_BaseContext_SQLServer context, CommonService CommonService, HttpClient http, IHubContext<LoggingHub> loggingHub) : base(context, CommonService, http, loggingHub)
        {
        }

        [HttpGet("GetRoleClaims")]
        // GET: api/RoleClaim/GetRoleClaims?IdRole=
        public async Task<CallbackResult<IList<GetRoleClaimViewModel>>> GetRoleClaims(string IdRole)
        {
            try
            {
                if (string.IsNullOrEmpty(IdRole))
                {
                    return new CallbackResult<IList<GetRoleClaimViewModel>>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var Role = await db_SQLServer.AspNetRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == IdRole);
                if (Role == null)
                {
                    return new CallbackResult<IList<GetRoleClaimViewModel>>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var RoleClaims = await db_SQLServer.AspNetRoleClaims.AsNoTracking()
                    .Select(x => new GetRoleClaimViewModel
                    {
                        Id = x.Id,
                        RoleId = x.RoleId,
                        ClaimType = x.ClaimType,
                        ClaimValue = x.ClaimValue,
                    })
                    .Where(x => x.RoleId == Role.Id)
                    .ToListAsync();


                return new CallbackResult<IList<GetRoleClaimViewModel>>(RoleClaims, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The DELETE statement conflicted with the SAME TABLE REFERENCE constraint"))
                {
                    return new CallbackResult<IList<GetRoleClaimViewModel>>(null, new DeleteError(DashboardResource.MessageDeleteConstraint));
                }
                else
                {
                    return new CallbackResult<IList<GetRoleClaimViewModel>>(null, new ExceptionError(ex.Message));
                }
            }
        }

        [HttpPost("CreateRange")]
        // POST: api/RoleClaim/CreateRange
        public async Task<CallbackResult<IList<SubmitRoleClaimViewModel>>> CreateRange(SubmitRoleClaimViewModel[] Models)
        {
            try
            {
                if (Models == null)
                {
                    return new CallbackResult<IList<SubmitRoleClaimViewModel>>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var AddRoleClaims = Models.Where(z => z.Id == 0 && z.IsSelected)
                    .Select(x => new IdentityRoleClaim<string>
                    {
                        ClaimType = x.ClaimType,
                        ClaimValue = x.ClaimValue,
                        Id = x.Id,
                        RoleId = x.RoleId,
                    });

                if (AddRoleClaims.Any())
                {
                    db_SQLServer.RoleClaims.AddRange(AddRoleClaims);
                }

                var RemoveRoleClaims = Models.Where(z => z.Id > 0 && !z.IsSelected)
                    .Select(x => new IdentityRoleClaim<string>
                    {
                        ClaimType = x.ClaimType,
                        ClaimValue = x.ClaimValue,
                        Id = x.Id,
                        RoleId = x.RoleId,
                    });

                if (RemoveRoleClaims.Any())
                {
                    db_SQLServer.RoleClaims.RemoveRange(RemoveRoleClaims);
                }

                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<IList<SubmitRoleClaimViewModel>>(Models, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    return new CallbackResult<IList<SubmitRoleClaimViewModel>>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else if (ex.ToString().Contains("The database operation was expected to affect 1 row(s)"))
                {
                    return new CallbackResult<IList<SubmitRoleClaimViewModel>>(null, new ExceptionError(DashboardResource.MessageChangeIsNotPossible));
                }
                else
                {
                    return new CallbackResult<IList<SubmitRoleClaimViewModel>>(null, new ExceptionError(ex.Message));
                }
            }
        }

    }
}
