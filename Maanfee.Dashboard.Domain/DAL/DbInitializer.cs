using Microsoft.EntityFrameworkCore;

namespace Maanfee.Dashboard.Domain.DAL
{
    public static class DbInitializer<TContext> where TContext : DbContext // _BaseEntityContext<TContext>, IBaseDbContext
    {
        public static void DatabaseCreating(TContext context)
        {
            context.Database.EnsureCreated();
        }

      
    }
}