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
using MudBlazor;

namespace Maanfee.Dashboard.Services.Controllers.Authentications
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RolesController : _BaseController<RolesController>
    {
        public RolesController(_BaseContext_SQLServer context
            , CommonService commonService
            , HttpClient http
            , ILogger<RolesController> logger
            , LoggingInitializer loggingInitializer
            , HubConnection loggingHubConnection
            ) : base(context, commonService, http, logger, loggingInitializer, loggingHubConnection)
        {
        }

        // Used : Roles->PaginationIndex
        [HttpPost("PaginationIndex")]
        // GET: api/Roles/PaginationIndex
        public async Task<CallbackResult<PaginatedListViewModel<IdentityRole>>> PaginationIndex(TableStateViewModel<FilterRoleViewModel> TableState)
        {
            try
            {
                PaginatedList<IdentityRole> PaginatedList;

                IQueryable<IdentityRole> Data = db_SQLServer.AspNetRoles.AsNoTracking().OrderBy(x => x.Name);

                switch (TableState.state.SortLabel)
                {
                    case "Name":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.Name);
                        break;
                    case "NormalizedName":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.NormalizedName);
                        break;
                }

                if (TableState.Filter != null)
                {
                    if (!string.IsNullOrEmpty(TableState.Filter.Role))
                    {
                        Data = Data.Where(p => p.Name.Contains(TableState.Filter.Role));
                    }
                    PaginatedList = await PaginatedList<IdentityRole>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }
                else
                {
                    PaginatedList = await PaginatedList<IdentityRole>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }

                return new CallbackResult<PaginatedListViewModel<IdentityRole>>(new PaginatedListViewModel<IdentityRole> { List = PaginatedList.List, TotalPages = PaginatedList.TotalPages, }, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<PaginatedListViewModel<IdentityRole>>(null, new ExceptionError(ex.ToString()));
            }
        }

        // Used : Roles->Create
        [HttpPost("Create")]
        // POST: api/Roles/Create
        public async Task<CallbackResult<IdentityRole>> Create(SubmitRoleViewModel Model)
        {
            try
            {
                var IdentityRole = new IdentityRole
                {
                    Name = Model.Role,
                    NormalizedName = Model.Role.ToUpper(),
                };
                db_SQLServer.AspNetRoles.Add(IdentityRole);

                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<IdentityRole>(IdentityRole, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    return new CallbackResult<IdentityRole>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<IdentityRole>(null, new ExceptionError(ex.Message));
                }
            }
        }

        // Used : Roles->Edit  (GetModel)
        // Used : Roles->Details 
        [HttpGet("Details/{Id}")]
        // GET: api/Roles/Details/5
        public async Task<CallbackResult<IdentityRole>> Details(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return new CallbackResult<IdentityRole>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var Model = await db_SQLServer.AspNetRoles.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Id);

                return new CallbackResult<IdentityRole>(Model, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IdentityRole>(null, new ExceptionError(ex.ToString()));
            }
        }

        // Used : Roles->Edit
        [HttpPut("Edit")]
        // Usage :   var Investment = await Http.PutAsJsonAsync("api/Roles/Edit", item.TrimString());
        //[HttpPut("{id}")]
        // PUT: api/Roles/Edit/Model
        public async Task<CallbackResult<IdentityRole>> Edit(SubmitRoleViewModel Model)
        {
            try
            {
                if (string.IsNullOrEmpty(Model.Id))
                {
                    return new CallbackResult<IdentityRole>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var IdentityRole = await db_SQLServer.AspNetRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Model.Id);
                if (IdentityRole == null)
                {
                    return new CallbackResult<IdentityRole>(null, new ExceptionError(DashboardResource.MessageChangeIsNotPossible));
                }

                IdentityRole.Name = Model.Role;
                IdentityRole.NormalizedName = Model.Role.ToUpper();

                //db_SQLServer.Entry(IdentityRole).State = EntityState.Modified;
                db_SQLServer.AspNetRoles.Update(IdentityRole);
                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<IdentityRole>(IdentityRole, null, DashboardResource.MessageSavedSuccessfully);
            }
            catch (DbUpdateException ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    return new CallbackResult<IdentityRole>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<IdentityRole>(null, new ExceptionError(ex.Message));
                }
            }
        }

        // Used : Roles->Delete        
        [HttpDelete("Delete/{Id}")]
        // GET: api/Roles/Delete/1
        public async Task<CallbackResult<IdentityRole>> Delete(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return new CallbackResult<IdentityRole>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var IdentityRole = await db_SQLServer.AspNetRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
                if (IdentityRole == null)
                {
                    return new CallbackResult<IdentityRole>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                db_SQLServer.Remove(IdentityRole);
                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<IdentityRole>(IdentityRole, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The DELETE statement conflicted with the "))
                {
                    return new CallbackResult<IdentityRole>(null, new DeleteError(DashboardResource.MessageDeleteConstraint));
                }
                else
                {
                    return new CallbackResult<IdentityRole>(null, new ExceptionError(ex.InnerException.Message));
                }
            }
        }

        // ***********************************************

        // Used : Roles->GetRole
        [HttpGet("GetRole")]
        // GET: api/Roles/GetRole
        public async Task<CallbackResult<IEnumerable<GetRoleViewModel>>> GetRole()
        {
            try
            {
                var list = await db_SQLServer.AspNetRoles.AsNoTracking()
                    .Select(x => new GetRoleViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NormalizedName = x.NormalizedName,
                    }).ToListAsync();

                return new CallbackResult<IEnumerable<GetRoleViewModel>>(list, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<GetRoleViewModel>>(null, new ExceptionError(ex.Message));
            }
        }

    }
}
