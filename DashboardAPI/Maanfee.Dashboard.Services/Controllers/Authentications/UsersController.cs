using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Maanfee.Dashboard.Resources;
using Maanfee.Logging.Console;
using Maanfee.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    public class UsersController : _BaseController<UsersController>
    {
        public UsersController(_BaseContext_SQLServer context
            , CommonService commonService
            , HttpClient http
            , ILogger<UsersController> logger
            , LoggingInitializer loggingInitializer
            , HubConnection loggingHubConnection
            ) : base(context, commonService, http, logger, loggingInitializer, loggingHubConnection)
        {
        }

        // Used : User->DetailsView
        [HttpGet("GetUserById/{Id}")]
        // GET: api/Users/DetailsView/fb0fc65b-9cb4-47b7-823d-94612b966adf
        public async Task<CallbackResult<GetUserViewModel>> GetUserById(string Id)
        {
            try
            {
                var user = await CommonService.GetUsers()
                    .Include(x => x.Gender)
                    .Include(x => x.UserDepartments).ThenInclude(x => x.Department)
                    .Include(x => x.UserGroups).ThenInclude(x => x.Group)
                    .AsSplitQuery()
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
                        UserGroupTitle = string.Join(" , ", x.UserGroups.Select(x => x.Group.Title)),
                    }).FirstOrDefaultAsync(x => x.Id == Id);

                if (user == null)
                {
                    return new CallbackResult<GetUserViewModel>(null, new ExceptionError(DashboardResource.MessageChangeIsNotPossible));
                }

                return new CallbackResult<GetUserViewModel>(user, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<GetUserViewModel>(null, new ExceptionError(ex.Message));
            }
        }

        // Used : AdminLayout
        [HttpGet("GetUserByUserName/{UserName}")]
        // GET: api/Users/GetUserByUserName/UserName
        public async Task<CallbackResult<ApplicationUser>> GetUserByUserName(string UserName)
        {
            try
            {
                var user = await CommonService.GetUsers().FirstOrDefaultAsync(x => x.UserName == UserName);

                if (user == null)
                {
                    return new CallbackResult<ApplicationUser>(null, new ExceptionError(DashboardResource.MessageChangeIsNotPossible));
                }

                return new CallbackResult<ApplicationUser>(user, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<ApplicationUser>(null, new ExceptionError(ex.Message));
            }
        }

        // Used : 
        [HttpGet("GetUserByIdDepartment")]
        // GET: api/Users/GetUserByIdDepartment?IdDepartment=&Value=
        public async Task<CallbackResult<IEnumerable<ApplicationUser>>> GetUserByIdDepartment(int IdDepartment, string Value)
        {
            try
            {
                var Users = await db_SQLServer.UserDepartments.AsNoTracking()
                    .Include(x => x.ApplicationUser)
                    .Where(x => x.IdDepartment == IdDepartment)
                    .Select(x => x.ApplicationUser)
                    .ToListAsync();

                if (!string.IsNullOrEmpty(Value))
                {
                    Users = Users.Where(x => x.Name.Contains(Value)).ToList();
                }

                return new CallbackResult<IEnumerable<ApplicationUser>>(Users, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<ApplicationUser>>(null, new ExceptionError(ex.Message));
            }
        }

        // **********************************************

        // Used : New ...
        [HttpGet("GetUserByIdGroup")]
        // GET: api/Users/GetUserByIdGroup?IdGroup=
        public async Task<CallbackResult<IEnumerable<UserGroupViewModel>>> GetUserByIdGroup(int IdGroup)
        {
            try
            {
                var Users = await db_SQLServer.UserGroups.AsNoTracking()
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Group)
                    .Where(x => x.IdGroup == IdGroup)
                    .Select(x => new UserGroupViewModel
                    {
                        GroupTitle = x.Group.Title,
                        IdGroup = x.IdGroup,
                        IdApplicationUser = x.IdApplicationUser,
                        Name = x.ApplicationUser.Name,
                        UserName = x.ApplicationUser.UserName,
                    }).ToListAsync();

                return new CallbackResult<IEnumerable<UserGroupViewModel>>(Users, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<UserGroupViewModel>>(null, new ExceptionError(ex.Message));
            }
        }

        // *******************************************

        // Used : Char   ???
        [HttpGet("GetUsers")]
        // GET: api/Users/GetUsers
        public async Task<CallbackResult<IEnumerable<GetUserViewModel>>> GetUsers(string Name)
        {
            try
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    return new CallbackResult<IEnumerable<GetUserViewModel>>(CommonService.GetVirtualUsers().Where(x => x.Name.Contains(Name)), null);
                }

                return new CallbackResult<IEnumerable<GetUserViewModel>>(await CommonService.GetVirtualUsersAsync(), null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<GetUserViewModel>>(null, new ExceptionError(ex.Message));
            }
        }

        [HttpPost("PaginationIndex")]
        // GET: api/Users/PaginationIndex?pageNumber=1&pageSize=10
        public async Task<CallbackResult<PaginatedList<GetUserViewModel>>> PaginationIndex(TableStateViewModel<FilterUserViewModel> TableState)
        {
            try
            {
                PaginatedList<GetUserViewModel> PaginatedList;

                IQueryable<GetUserViewModel> Data = CommonService.GetQueryableVirtualUsers().OrderBy(x => x.Name);

                switch (TableState.state.SortLabel)
                {
                    case "Name":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.Name);
                        break;
                    case "UserName":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.UserName);
                        break;
                    case "PersonalCode":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.PersonalCode);
                        break;
                    case "RoleName":
                        Data = Data.OrderByDirection(TableState.state.SortDirection, o => o.RoleName);
                        break;
                }

                if (TableState.Filter != null)
                {
                    if (!string.IsNullOrEmpty(TableState.Filter.Name))
                    {
                        Data = Data.Where(x => x.Name.Contains(TableState.Filter.Name));
                    }
                    if (TableState.Filter.Department != null)
                    {
                        Data = Data.Where(x => x.UserDepartments.Any(a => TableState.Filter.Department.Id == a.IdDepartment));
                    }

                    PaginatedList = await PaginatedList<GetUserViewModel>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }
                else
                {
                    PaginatedList = await PaginatedList<GetUserViewModel>.CreateAsync(Data, TableState.state.Page, TableState.state.PageSize);
                }

                return new CallbackResult<PaginatedList<GetUserViewModel>>(PaginatedList, null);
            }
            catch (Exception ex)
            {
                if (LoggingHubConnection is not null)
                {
                    await LoggingHubConnection.SendAsync("SendMessageAsync", new LogInfo
                    {
                        Platform = LoggingPlatformDefaultValue.Server,
                        Message = ex.ToString(),
                        LogDate = DateTime.Now,
                        Level = Maanfee.Logging.Console.LogLevel.Error,
                    });
                }

                return new CallbackResult<PaginatedList<GetUserViewModel>>(null, new ExceptionError(ex.ToString()));
            }
        }

        // Used : Users->Create|Edit
        [HttpGet("GetGenders")]
        // GET: api/Users/GetGenders
        public async Task<CallbackResult<IEnumerable<Gender>>> GetGenders()
        {
            try
            {
                var list = await db_SQLServer.Genders.AsNoTracking()
                    .Select(x => new Gender
                    {
                        Id = x.Id,
                        Sex = x.Sex,
                    })
                    .ToListAsync();

                return new CallbackResult<IEnumerable<Gender>>(list, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IEnumerable<Gender>>(null, new ExceptionError(ex.Message));
            }
        }

        [HttpGet("GetAvailableDepartmentByIdDepartment")]
        // GET: api/Users/GetAvailableDepartmentByIdDepartment?IdDepartment=4
        public async Task<CallbackResult<IList<DropDownDepartmentViewModel>>> GetAvailableDepartmentByIdDepartment(int? IdDepartment, string Value)
        {
            try
            {
                if (!IdDepartment.HasValue)
                {
                    return new CallbackResult<IList<DropDownDepartmentViewModel>>(null, new ExceptionError(DashboardResource.MessageChangeIsNotPossible));
                }

                var Me = await db_SQLServer.UserDepartments.AsNoTracking()
                        .Include(x => x.Department)
                        .Include(x => x.Department).ThenInclude(x => x.Department1)
                        .Include(x => x.Department).ThenInclude(x => x.Department2)
                        .Where(x => x.IdDepartment == IdDepartment)
                        .FirstOrDefaultAsync();

                var AvailableDepartment = new List<DropDownDepartmentViewModel>();

                if (Me.Department.Department2 != null)
                {
                    // Father
                    DropDownDepartmentViewModel Father = null;

                    Father = new DropDownDepartmentViewModel()
                    {
                        Id = Me.Department.Department2.Id,
                        IdParent = (Me.Department.Department2 != null) ? Me.Department.Department2.IdParent : null,
                        Title = (Me.Department.Department2 != null) ? Me.Department.Department2.Title : "",
                    };
                    AvailableDepartment.Add(Father);

                    // Brothers
                    foreach (var item in await db_SQLServer.Departments
                        .Where(x => x.IdParent == ((Father != null) ? Father.Id : null)).ToListAsync())
                    {
                        AvailableDepartment.Add(new DropDownDepartmentViewModel()
                        {
                            Id = item.Id,
                            IdParent = item.IdParent,
                            Title = item.Title,
                        });
                    }
                }

                // Sons
                foreach (var item in Me.Department.Department1)
                {
                    AvailableDepartment.Add(new DropDownDepartmentViewModel()
                    {
                        Id = item.Id,
                        IdParent = item.IdParent,
                        Title = item.Title,
                    });
                }

                if (!string.IsNullOrEmpty(Value))
                {
                    AvailableDepartment = AvailableDepartment.Where(x => x.Title.Contains(Value)).ToList();
                }

                return new CallbackResult<IList<DropDownDepartmentViewModel>>(AvailableDepartment, null);
            }
            catch (Exception ex)
            {
                return new CallbackResult<IList<DropDownDepartmentViewModel>>(null, new ExceptionError(ex.Message));
            }
        }

        // ***************************************************

    }
}
