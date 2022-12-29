using Maanfee.Dashboard.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Maanfee.Dashboard.Domain.DAL
{
    public abstract class _BaseEntityContext<T> : IdentityDbContext<ApplicationUser> where T : DbContext, IBaseDbContext
    {
        public _BaseEntityContext(DbContextOptions<T> options) : base(options)
        {
        }
    }
}
