using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Maanfee.Dashboard.Services
{
    public static class ServerServicesExtensions
    {
        public static IServiceCollection AddServerServices(this IServiceCollection services, IConfiguration Configuration
            , string SQLServerConnectionString)
        {
            // Add Databases services.
            #region - Configuring Database -

            services.AddDbContext<_BaseContext_SQLServer>(options => options.UseSqlServer(Configuration.GetConnectionString(SQLServerConnectionString)));
            services.AddDbContext<_BaseContext_InMemory>(options => options.UseInMemoryDatabase("InMemoryConnection"));
            services.AddDbContext<_BaseContext_SQLite>(options => options.UseSqlite("Filename=SQLite_db.db"));

            #endregion

            #region - Identity -

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                //options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(9);
                options.Lockout.MaxFailedAccessAttempts = 1;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<_BaseContext_SQLServer>()
                .AddDefaultTokenProviders();

            #endregion

            services.ConfigureApplicationCookie(options =>
            {
                //options.Cookie.HttpOnly = false;
                //options.Events.OnRedirectToLogin = context =>
                //{
                //    context.Response.StatusCode = 401;
                //    return Task.CompletedTask;
                //};
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(3600);

                // Cookie settings
                options.LoginPath = "/login";
                options.LogoutPath = "/login";
                options.AccessDeniedPath = "/login";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(9);
            });

            // Fixed Self referencing loop detected with type
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );

            services.AddScoped<HttpClient>();
            return services.AddScoped<CommonService>();
        }

    }
}
