using Maanfee.Dashboard.Domain.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Maanfee.Dashboard.Services
{ 
    public static class DatabaseConfigureExtensions
    {
        public static IApplicationBuilder AddConfigure(this IApplicationBuilder app, IWebHostEnvironment env,
           _BaseContext_SQLServer SQLServerContext, _BaseContext_SQLite SQLiteContext)
        {
            if (env.IsDevelopment())
            {
                DatabaseCreating<_BaseContext_SQLServer>(SQLServerContext);
                DatabaseCreating<_BaseContext_SQLite>(SQLiteContext);

                SQLServerDataInitializer.Initialize(SQLServerContext);
                SQLiteDataInitializer.Initialize(SQLiteContext);
            }

            return app;
        }

        private static void DatabaseCreating<TContext>(TContext context) where TContext : DbContext
        {
            context.Database.EnsureCreated();
        }
    }
}
