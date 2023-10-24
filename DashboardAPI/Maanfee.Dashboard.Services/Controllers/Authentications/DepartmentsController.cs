using Maanfee.Dashboard.Domain;
using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Logging.Console;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
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
    public class DepartmentsController : _BaseController
    {
        public DepartmentsController(_BaseContext_SQLServer context, CommonService CommonService, HttpClient http, IHubContext<LoggingHub> loggingHub) : base(context, CommonService, http, loggingHub)
        {
        }

        // Used : Departments->Index
        [HttpGet("Index")]
        // GET: api/Departments/Index
        public async Task<CallbackResult<HashSet<Department>>> Index()
        {
            try
            {
                await Task.Delay(10);
                var list = db_SQLServer.Departments.ToHashSet();

                HashSet<Department> Departments = new HashSet<Department>();

                foreach (var item in list)
                {
                    if (item.IdParent == null)
                    {
                        Departments.Add(item);
                    }
                }

                return new CallbackResult<HashSet<Department>>(Departments, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<HashSet<Department>>(null, new ExceptionError(ex.Message));
            }
        }

        // Used : Departments->GetDepartments
        [HttpGet("GetDepartments")]
        // GET: api/Departments/GetDepartments?value=
        public async Task<CallbackResult<List<Department>>> GetDepartments(string value)
        {
            try
            {
                var list = await db_SQLServer.Departments.AsNoTracking().ToListAsync();

                if (string.IsNullOrEmpty(value))
                    return new CallbackResult<List<Department>>(list, null);

                list = list.Where(x => x.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase)).ToList();

                return new CallbackResult<List<Department>>(list, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<List<Department>>(null, new ExceptionError(ex.Message));
            }
        }

        #region -  -

        // Used : Departments->GetDropDownDepartments
        [HttpGet("GetDropDownDepartments")]
        // GET: api/Departments/GetDropDownDepartments?value=
        public async Task<CallbackResult<List<DropDownDepartmentViewModel>>> GetDropDownDepartments(string value)
        {
            try
            {
                var list = await db_SQLServer.Departments.Select(x => new DropDownDepartmentViewModel
                {
                    Id = x.Id,
                    IdParent = x.IdParent,
                    Title = x.Title,
                }).ToListAsync();

                if (string.IsNullOrEmpty(value))
                    return new CallbackResult<List<DropDownDepartmentViewModel>>(list, null);

                list = list.Where(x => x.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase)).ToList();

                return new CallbackResult<List<DropDownDepartmentViewModel>>(list, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<List<DropDownDepartmentViewModel>>(null, new ExceptionError(ex.Message));
            }
        }

        // Used : Users->Crudate
        [HttpGet("GetIdUserDepartments")]
        // GET: api/Departments/GetIdUserDepartments?IdUser=&IsPersonal=
        public async Task<CallbackResult<IEnumerable<int>>> GetIdUserDepartments(string IdUser)
        {
            try
            {
                var IdDepartments = await db_SQLServer.UserDepartments
                    .Where(x => x.IdApplicationUser == IdUser)
                    .Select(x => x.IdDepartment)
                    .ToListAsync();

                return new CallbackResult<IEnumerable<int>>(IdDepartments, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<int>>(null, new ExceptionError(ex.Message));
            }
        }

        #endregion

        // Used : Dropdownlist
        [HttpGet("GetUserDepartments")]
        // GET: api/Departments/GetUserDepartments?IdUser=
        public async Task<CallbackResult<IEnumerable<UserDepartment>>> GetUserDepartments(string IdUser)
        {
            try
            {
                var IdDepartments = await db_SQLServer.UserDepartments.AsNoTracking()
                    .Include(x => x.Department)
                    .Where(x => x.IdApplicationUser == IdUser)
                    .ToListAsync();

                return new CallbackResult<IEnumerable<UserDepartment>>(IdDepartments, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<UserDepartment>>(null, new ExceptionError(ex.Message));
            }
        }

        // Used : Departments->Create|Update
        [HttpPost("CreateOrUpdate")]
        // POST: api/Departments/CreateOrUpdate
        public async Task<CallbackResult<Department>> CreateOrUpdate(SubmitDepartmentViewModel Model)
        {
            try
            {
                var Department = new Department();

                //return new CallbackResult<Department>(null, new ExceptionError(Model.Parent?.Id + "||||||"));

                if (!Model.Id.HasValue || Model.Id == 0)
                {
                    if (Model.Parent != null)
                    {
                        Department.IdParent = Model.Parent?.Id;
                    }
                    Department.Title = Model.Title;

                    // ********* PK *********
                    int PK = 1;
                    var dep = await db_SQLServer.Departments.ToListAsync();
                    if (dep.Any())
                    {
                        PK = dep.Max(x => x.Id);
                    }
                    Department.Id = ++PK;
                    // **********************

                    db_SQLServer.Departments.Add(Department);
                }
                else
                {
                    if (Model.Parent != null)
                    {
                        if (Model.Id == Model.Parent.Id)
                        {
                            return new CallbackResult<Department>(null, new ExceptionError(DashboardResource.MessageThisItemCouldNoBeASubsetOfItself));
                        }
                    }

                    Department = await db_SQLServer.Departments.FirstOrDefaultAsync(x => x.Id == Model.Id);
                    if (Department == null)
                    {
                        return new CallbackResult<Department>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                    }

                    Department.IdParent = Model.Parent?.Id ?? null;
                    Department.Title = Model.Title;

                    db_SQLServer.Departments.Update(Department);
                }

                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<Department>(Department, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot insert duplicate key row in object"))
                {
                    return new CallbackResult<Department>(null, new DuplicateError(DashboardResource.MessageCannotInsertDuplicate));
                }
                else
                {
                    return new CallbackResult<Department>(null, new ExceptionError(ex.Message));
                }
            }
        }

        [HttpDelete("Delete/{Id}")]
        // GET: api/Departments/Delete/1
        public async Task<CallbackResult<Department>> Delete(int? Id)
        {
            try
            {
                if (Id == null)
                {
                    return new CallbackResult<Department>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                var Department = await db_SQLServer.Departments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
                if (Department == null)
                {
                    return new CallbackResult<Department>(null, new Error(ErrorCode.ChangeIsNotPossible, DashboardResource.MessageChangeIsNotPossible));
                }

                db_SQLServer.Remove(Department);
                await db_SQLServer.SaveChangesAsync();

                return new CallbackResult<Department>(Department, null);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The DELETE statement conflicted with the "))
                {
                    return new CallbackResult<Department>(null, new DeleteError(DashboardResource.MessageDeleteConstraint));
                }
                else
                {
                    return new CallbackResult<Department>(null, new ExceptionError(ex.InnerException.Message));
                }
            }
        }

        // *****************************************************

    }
}
