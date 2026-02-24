using Maanfee.Dashboard.Domain.DAL;
using Maanfee.Dashboard.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Maanfee.Dashboard.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDashboardCompression(this IServiceCollection Services)
        {
            return Services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
            });
        }

        public static IServiceCollection AddDashboardDatabaseConfigurations(this IServiceCollection Services, IConfiguration Configuration)
        {
            var SQLServerConnectionString = Configuration.GetConnectionString("SQLServerConnection_DebugMode") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            #region - Configuring Database -

            Services.AddDbContext<_BaseContext_SQLServer>(options => options.UseSqlServer(SQLServerConnectionString));
            Services.AddDbContext<_BaseContext_InMemory>(options => options.UseInMemoryDatabase("InMemoryConnection"));
            Services.AddDbContext<_BaseContext_SQLite>(options => options.UseSqlite("Filename=SQLite_db.db"));

            Services.AddDatabaseDeveloperPageExceptionFilter();

            #endregion

            #region - Identity -

            Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                //options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.MaxFailedAccessAttempts = 5;  // ۵ تلاش ناموفق مجاز
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // ۱۵ دقیقه قفل
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<_BaseContext_SQLServer>()
                .AddDefaultTokenProviders();

            #endregion

            return Services;
        }

        public static IServiceCollection AddDashboardApplicationCookieConfigurations(this IServiceCollection Services)
        {
            return Services.ConfigureApplicationCookie(options =>
            {
                //options.Events.OnRedirectToLogin = context =>
                //{
                //    context.Response.StatusCode = 401;
                //    return Task.CompletedTask;
                //};

                // Cookie settings
                options.LoginPath = "/login";
                options.LogoutPath = "/login";
                options.AccessDeniedPath = "/login";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(8);

                // این تنظیمات برای InteractiveServer مهم هستند
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });
        }

        public static IMvcBuilder AddDashboardControllers(this IServiceCollection Services)
        {
            return Services.AddControllers().AddJsonOptions(options =>
            {
                // جلوگیری از self referencing loop
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                // حساس نبودن به حروف بزرگ و کوچک
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;

                //// استفاده از CamelCase برای نام خواص
                //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                //// نادیده گرفتن خواص null
                //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                //// پشتیبانی از تبدیل Enum به string
                //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

    }
} 
