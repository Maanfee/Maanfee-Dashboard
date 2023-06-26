using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Services
{
    public class CommonService
    {
        public CommonService(_BaseContext_SQLServer context, _BaseContext_SQLite SQLiteContext)
        {
            db_SQLServer = context;
            db_SQLite = SQLiteContext;
        }

        protected readonly _BaseContext_SQLServer db_SQLServer;
        protected readonly _BaseContext_SQLite db_SQLite;

        // ******************************************************************

        #region - Users -

        public IQueryable<ApplicationUser> GetUsers()
        {
            return db_SQLServer.ApplicationUsers.AsNoTracking()
                .Include(x => x.UserDepartments);
        }

        public IQueryable<GetUserViewModel> GetQueryableVirtualUsers()
        {
            var UserRoles = db_SQLServer.AspNetUserRoles.AsNoTracking();
            var list = db_SQLServer.Users
                .Include(x => x.UserDepartments)
                .Select(x => new GetUserViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    UserName = x.UserName,
                    PersonalCode = x.PersonalCode,
                    RoleName = db_SQLServer.AspNetRoles.FirstOrDefault(a => a.Id == (db_SQLServer.AspNetUserRoles.FirstOrDefault(z => z.UserId == x.Id)).RoleId).Name ?? "-",
                    Avatar = x.Avatar,
                    UserDepartmentsTitle = string.Join(" , ", x.UserDepartments.Select(x => x.Department.Title)),
                });

            return list;
        }

        public async Task<IEnumerable<GetUserViewModel>> GetVirtualUsersAsync()
        {
            var UserRoles = db_SQLServer.AspNetUserRoles.AsNoTracking().ToList();
            var list = await db_SQLServer.Users
                .Select(x => new GetUserViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    UserName = x.UserName,
                    PersonalCode = x.PersonalCode,
                    RoleName = db_SQLServer.AspNetRoles.FirstOrDefault(a => a.Id == (db_SQLServer.AspNetUserRoles.FirstOrDefault(z => z.UserId == x.Id)).RoleId).Name ?? "-",
                    Avatar = x.Avatar,
                }).ToListAsync();

            return list;
        }

        public IEnumerable<GetUserViewModel> GetVirtualUsers() => GetVirtualUsersAsync().Result;

        #endregion

        // ******************************************************************

    }
}
