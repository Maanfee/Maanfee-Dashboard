using Maanfee.Dashboard.Domain.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
 
namespace Maanfee.Dashboard.Services
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseDashboardCompression(this IApplicationBuilder app)
        {
            return app.UseResponseCompression();  // فشرده‌سازی
        }

        public static IApplicationBuilder UseDashboardAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthentication();    // اول احراز هویت
            app.UseAuthorization();     // سپس مجوزدهی

            return app;  
        }

        public static IApplicationBuilder UseDashboardDatabaseConfigurations(this WebApplication app)
        { 
            if (app.Environment.IsDevelopment())
            {
                var scope = app.Services.CreateScope();
                var SQLServerContext = scope.ServiceProvider.GetRequiredService<_BaseContext_SQLServer>();
                var SQLiteContext = scope.ServiceProvider.GetRequiredService<_BaseContext_SQLite>();

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

        public static ControllerActionEndpointConventionBuilder UseDashboardControllers(this IEndpointRouteBuilder endpoints)
        {
            // این خط مهم است - برای اینکه APIها به درستی کار کنند
            return endpoints.MapControllers(); 
        }

    }
}
