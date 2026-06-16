using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Logging.Console;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Maanfee.Dashboard.Services.Controllers.Authentications
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PermissionsController : _BaseController<GroupsController>
    {
        public PermissionsController(_BaseContext_SQLServer context
            , CommonService CommonService
            , HttpClient http
            , ILogger<GroupsController> logger
            , LoggingInitializer loggingInitializer
            , HubConnection loggingHubConnection
            ) : base(context, CommonService, http, logger, loggingInitializer, loggingHubConnection)
        {
        }

        // Used : Permissions->Create|Update
        [HttpPost("CreateOrUpdate")]
        // POST: api/Permissions/CreateOrUpdate
        public async Task<CallbackResult<Permission>> CreateOrUpdate(SubmitPermissionViewModel Model)
        {
            try
            {
                var Permission = new Permission();

                //return new CallbackResult<Permission>(null, new ExceptionError(Model.Parent?.Id + "||||||"));

                if (string.IsNullOrEmpty(Model.Id))
                {
                    if (Model.Parent != null)
                    {
                        Permission.IdParent = Model.Parent?.Id;
                    }
                    Permission.Id = Guid.NewGuid().ToString();
                    Permission.Title = Model.Title!;
                    Permission.DisplayTitle = Model.DisplayTitle!;
                    Permission.FullName = Model.FullName!;

                    db_SQLServer!.Permissions.Add(Permission);
                }
                else
                {
                    if (Model.Parent != null)
                    {
                        if (Model.Id == Model.Parent.Id)
                        {
                            return new CallbackResult<Permission>(null, new ExceptionError(DashboardResource.MessageThisItemCouldNoBeASubsetOfItself));
                        }
                    }

                    Permission = await db_SQLServer!.Permissions.FirstOrDefaultAsync(x => x.Id == Model.Id);
                    if (Permission == null)
                    {
                        return new CallbackResult<Permission>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                    }

                    Permission.IdParent = Model.Parent?.Id ?? null;
                    Permission.Title = Model.Title!;
                    Permission.DisplayTitle = Model.DisplayTitle!;
                    Permission.FullName = Model.FullName!;

                    db_SQLServer.Permissions.Update(Permission);
                }

                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<Permission>(Permission, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    return new CallbackResult<Permission>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<Permission>(null, new ExceptionError(ex.Message));
                }
            }
        }

        // Used : Permissions->Delete        
        [HttpDelete("Delete/{Id}")]
        // GET: api/Permissions/Delete/1
        public async Task<CallbackResult<Permission>> Delete(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return new CallbackResult<Permission>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var Model = await db_SQLServer!.Permissions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
                if (Model == null)
                {
                    return new CallbackResult<Permission>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                db_SQLServer.Remove(Model);
                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<Permission>(Model, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The DELETE statement conflicted with the "))
                {
                    return new CallbackResult<Permission>(null, new DeleteError(DashboardResource.MessageDeleteConstraint));
                }
                else
                {
                    return new CallbackResult<Permission>(null, new ExceptionError(ex.InnerException!.Message));
                }
            }
        }

        #region - Get DropDown Permissions -

        // Used : Permissions->GetDropDownPermissions
        [HttpGet("GetDropDownPermissions")]
        // GET: api/Permissions/GetDropDownPermissions?value=
        public async Task<CallbackResult<List<DropDownPermissionViewModel>>> GetDropDownPermissions(string? value)
        {
            try
            {
                var list = await db_SQLServer!.Permissions.Select(x => new DropDownPermissionViewModel
                    {
                        Id = x.Id,
                        IdParent = x.IdParent,
                        Title = $"{x.Title} - {x.DisplayTitle}",
                        Name = x.Title,
                    }).ToListAsync();

                if (string.IsNullOrEmpty(value))
                    return new CallbackResult<List<DropDownPermissionViewModel>>(list, null);

                list = list.Where(x => x.Title!.Contains(value, StringComparison.InvariantCultureIgnoreCase)).ToList();

                return new CallbackResult<List<DropDownPermissionViewModel>>(list, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<List<DropDownPermissionViewModel>>(null, new ExceptionError(ex.Message));
            }
        }

        #endregion

        // Tree
        [HttpGet("GetPermissions")]
        // GET: api/Permissions/GetPermissions
        public async Task<CallbackResult<List<Permission>>> GetPermissions()
        {
            try
            {
                var list = await db_SQLServer!.Permissions.AsNoTracking().ToListAsync();

                return new CallbackResult<List<Permission>>(list, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<List<Permission>>(null, new ExceptionError(ex.Message));
            }
        }
    }
}
