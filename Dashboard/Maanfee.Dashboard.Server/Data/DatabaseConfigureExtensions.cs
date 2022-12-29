using Maanfee.Dashboard.Domain.DAL;
using Microsoft.AspNetCore.Builder;

namespace Maanfee.Dashboard.Server.Data
{
    public static class DatabaseConfigureExtensions
    {
        public static IApplicationBuilder AddConfigure(this IApplicationBuilder app,
           _BaseContext_SQLServer SQLServerContext, _BaseContext_SQLite SQLiteContext)
        {

#if DEBUG
            DbInitializer<_BaseContext_SQLServer>.DatabaseCreating(SQLServerContext);
            DbInitializer<_BaseContext_SQLite>.DatabaseCreating(SQLiteContext);

            SQLServerDataInitializer.Initialize(SQLServerContext);
            SQLiteDataInitializer.Initialize(SQLiteContext);
#endif

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
