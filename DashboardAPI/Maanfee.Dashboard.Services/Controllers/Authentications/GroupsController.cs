using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
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
    public class GroupsController : _BaseController
    {
        public GroupsController(_BaseContext_SQLServer context, CommonService CommonService, HttpClient http) : base(context, CommonService, http)
        {
        }

        // Used : Groups->PaginationIndex
        [HttpPost("PaginationIndex")]
        // GET: api/Groups/PaginationIndex
        public async Task<CallbackResult<PaginatedListViewModel<Group>>> PaginationIndex(TableStateViewModel<FilterGroupViewModel> TableState)
        {
            try
            {
                PaginatedList<Group> PaginatedList;

                IQueryable<Group> Data = db_SQLServer.Groups.AsNoTracking();

                switch (TableState.state.SortLabel)
                {
                    case "Title":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.Title);
                        break;
                }

                if (TableState.Filter != null)
                {
                    if (!string.IsNullOrEmpty(TableState.Filter.Title))
                    {
                        Data = Data.Where(p => p.Title.Contains(TableState.Filter.Title));
                    }
                    PaginatedList = await PaginatedList<Group>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }
                else
                {
                    PaginatedList = await PaginatedList<Group>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }

				return new CallbackResult<PaginatedListViewModel<Group>>(new PaginatedListViewModel<Group> { List = PaginatedList.List, TotalPages = PaginatedList.TotalPages, }, null);
			}
			catch (Exception ex)
            {
                return new CallbackResult<PaginatedListViewModel<Group>>(null, new ExceptionError(ex.ToString()));
            }
        }

        // Used : Groups->Create
        [HttpPost("Create")]
        // POST: api/Groups/Create
        public async Task<CallbackResult<SubmitGroupViewModel>> Create(SubmitGroupViewModel Model)
        {
            try
            {
                var Group = new Group
                {
                    Title = Model.Title,
                };
                db_SQLServer.Groups.Add(Group);

                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<SubmitGroupViewModel>(Model, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    return new CallbackResult<SubmitGroupViewModel>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<SubmitGroupViewModel>(null, new ExceptionError(ex.Message));
                }
            }
        }

        // Used : Groups->Edit  (GetModel)
        // Used : Groups->Details 
        [HttpGet("Details/{Id}")]
        // GET: api/Groups/Details/5
        public async Task<CallbackResult<Group>> Details(int? Id)
        {
            try
            {
                if (!Id.HasValue)
                {
                    return new CallbackResult<Group>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var Model = await db_SQLServer.Groups.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Id);

                return new CallbackResult<Group>(Model, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<Group>(null, new ExceptionError(ex.ToString()));
            }
        }

        // Used : Groups->Edit
        [HttpPut("Edit")]
        // Usage :   var Investment = await Http.PutAsJsonAsync("api/Roles/Edit", item.TrimString());
        //[HttpPut("{id}")]
        // PUT: api/Groups/Edit/Model
        public async Task<CallbackResult<SubmitGroupViewModel>> Edit(SubmitGroupViewModel Model)
        {
            try
            {
                var Details = await db_SQLServer.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Model.Id);
                if (Details == null)
                {
                    return new CallbackResult<SubmitGroupViewModel>(null, new ExceptionError(DashboardResource.MessageChangeIsNotPossible));
                }

                Details.Title = Model.Title;

                //db_SQLServer.Entry(Details).State = EntityState.Modified;
                db_SQLServer.Groups.Update(Details);
                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<SubmitGroupViewModel>(Model, null, DashboardResource.MessageSavedSuccessfully);
            }
            catch (DbUpdateException ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    return new CallbackResult<SubmitGroupViewModel>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<SubmitGroupViewModel>(null, new ExceptionError(ex.Message));
                }
            }
        }

        // Used : Groups->Delete        
        [HttpDelete("Delete/{Id}")]
        // GET: api/Groups/Delete/1
        public async Task<CallbackResult<Group>> Delete(int? Id)
        {
            try
            {
                if (!Id.HasValue)
                {
                    return new CallbackResult<Group>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var IdentityRole = await db_SQLServer.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
                if (IdentityRole == null)
                {
                    return new CallbackResult<Group>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                db_SQLServer.Remove(IdentityRole);
                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<Group>(IdentityRole, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The DELETE statement conflicted with the "))
                {
                    return new CallbackResult<Group>(null, new DeleteError(DashboardResource.MessageDeleteConstraint));
                }
                else
                {
                    return new CallbackResult<Group>(null, new ExceptionError(ex.InnerException.Message));
                }
            }
        }

        // *****************************************************

        #region -  -

        // Used : Groups->GetDropDownGroups
        [HttpGet("GetDropDownGroups")]
        // GET: api/Groups/GetDropDownGroups?value=
        public async Task<CallbackResult<List<DropDownGroupViewModel>>> GetDropDownGroups(string value)
        {
            try
            {
                var list = await db_SQLServer.Groups.Select(x => new DropDownGroupViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                }).ToListAsync();

                if (string.IsNullOrEmpty(value))
                    return new CallbackResult<List<DropDownGroupViewModel>>(list, null);

                list = list.Where(x => x.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase)).ToList();

                return new CallbackResult<List<DropDownGroupViewModel>>(list, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<List<DropDownGroupViewModel>>(null, new ExceptionError(ex.Message));
            }
        }

        // Used : Users->Crudate
        [HttpGet("GetIdUserGroups")]
        // GET: api/Groups/GetIdUserGroups?IdUser=
        public async Task<CallbackResult<IEnumerable<int>>> GetIdUserGroups(string IdUser)
        {
            try
            {
                var IdGroup = await db_SQLServer.UserGroups
                    .Where(x => x.IdApplicationUser == IdUser)
                    .Select(x => x.IdGroup)
                    .ToListAsync();

                return new CallbackResult<IEnumerable<int>>(IdGroup, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<int>>(null, new ExceptionError(ex.Message));
            }
        }



        #endregion

    }
}
