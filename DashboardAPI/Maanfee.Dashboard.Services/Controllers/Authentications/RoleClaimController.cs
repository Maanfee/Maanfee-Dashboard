using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Logging.Console;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Maanfee.Dashboard.Services.Controllers.Authentications
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RoleClaimController(_BaseContext_SQLServer context,
        CommonService commonService,
        HttpClient http,
        ILogger<RoleClaimController> logger,
        LoggingInitializer loggingInitializer,
        HubConnection loggingHubConnection) : _BaseController<RoleClaimController>(context,
            commonService, http, logger, loggingInitializer, loggingHubConnection)
    {

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

                var Role = await db_SQLServer!.AspNetRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == IdRole);
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
        public async Task<CallbackResult<IList<SubmitRoleClaimViewModel>>> CreateRange(IList<SubmitRoleClaimViewModel> SubmitModels)
        {
            try
            {
                var strategy = db_SQLServer!.Database.CreateExecutionStrategy();
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await db_SQLServer.Database.BeginTransactionAsync();
                    try
                    {
                        var ExistingRoleClaims = await db_SQLServer!.RoleClaims.Where(x => x.RoleId == SubmitModels.FirstOrDefault()!.RoleId).ToListAsync();
                        if (ExistingRoleClaims.Any())
                        {
                            db_SQLServer!.RoleClaims.RemoveRange(ExistingRoleClaims);
                        }

                        var NewRoleClaims = SubmitModels.Select(x => new IdentityRoleClaim<string>
                        {
                            ClaimType = x.ClaimType,
                            ClaimValue = x.ClaimValue,
                            RoleId = x.RoleId!,
                        });
                        if (NewRoleClaims.Any())
                        {
                            db_SQLServer!.RoleClaims.AddRange(NewRoleClaims);
                        }

                        await db_SQLServer!.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return new CallbackResult<IList<SubmitRoleClaimViewModel>>(SubmitModels, null);
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return new CallbackResult<IList<SubmitRoleClaimViewModel>>(SubmitModels, null);
                    }
                });
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
                    return new CallbackResult<IList<SubmitRoleClaimViewModel>>(null, new ExceptionError(ex.Message + " | " + ex.InnerException));
                }
            }
        }

        [HttpGet("GetRoleClaimsByUserId")]
        // GET: api/RoleClaim/GetRoleClaimsByUserId?IdUser=
        public async Task<CallbackResult<IList<string>>> GetRoleClaimsByUserId(string IdUser)
        {
            try
            {
                if (string.IsNullOrEmpty(IdUser))
                {
                    return new CallbackResult<IList<string>>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var Role = await db_SQLServer!.AspNetUserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == IdUser);
                if (Role == null)
                {
                    return new CallbackResult<IList<string>>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var RoleClaimsType = await db_SQLServer
         .AspNetRoleClaims
         .AsNoTracking()
         .Where(x => x.RoleId == Role.RoleId)
         .Select(x => x.ClaimType!)
         .Where(claimType => !string.IsNullOrEmpty(claimType))
         .Distinct()
         .ToListAsync();

                return new CallbackResult<IList<string>>(RoleClaimsType, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The DELETE statement conflicted with the SAME TABLE REFERENCE constraint"))
                {
                    return new CallbackResult<IList<string>>(null, new DeleteError(DashboardResource.MessageDeleteConstraint));
                }
                else
                {
                    return new CallbackResult<IList<string>>(null, new ExceptionError(ex.Message));
                }
            }
        }
    }
}
