using Maanfee.Dashboard.Views.Core.Services;
using Maanfee.Highcharts;
using Maanfee.JsInterop;
using Maanfee.Logging.Console;
using Maanfee.Lottie;
using Maanfee.Web.Core;
using Maanfee.Web.Printing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System.Reflection;

namespace Maanfee.Dashboard.Views.Core
{
    public static class ServiceCollectionExtensions
    {
        public static WebAssemblyHostBuilder AddDashboardClientServices(this WebAssemblyHostBuilder builder)
        {
            builder.Services.AddScoped(sp =>
            {
                var navigationManager = sp.GetRequiredService<NavigationManager>();
                return new HttpClient
                {
                    BaseAddress = new Uri(navigationManager.BaseUri)
                };
            });

            #region MudBlazor Snackbar Configuration

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;

                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = false;
                config.SnackbarConfiguration.VisibleStateDuration = 5000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            #endregion

            #region - Authentication -

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore(options =>
            {
                RegisterPermissionClaims(options);
            });
            builder.Services.AddScoped<CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());
            builder.Services.AddScoped<JwtAuthenticationStateProvider>();

            #endregion

            #region - Services -

            builder.Services.AddSingleton<AccountStateContainer>();
            builder.Services.AddSingleton<UrlStateContainer>();
            builder.Services.AddSingleton<LocalConfigurationService>();
            builder.Services.AddSingleton<PermissionService>();
            builder.Services.AddSingleton<TableConfigurationService>();

            #endregion

            // HighCharts
            builder.Services.AddHighCharts();

            // Print
            builder.Services.AddScoped<IPrintingService, PrintingService>();

            // File Upload
            builder.Services.AddScoped<IFilesManagerService, FilesManagerService>();

            // RealTime Logging
            builder.Services.AddLoggingConsole();

            // Lottie
            builder.Services.AddMaanfeeLottie();

            // JsInterop
            builder.Services.AddMaanfeeJsInterop();

            return builder;
        }

        //public static IServiceCollection AddDashboardAuthorization(this IServiceCollection Services)
        //{


        //    return Services;
        //}

        private static void RegisterPermissionClaims(AuthorizationOptions options)
        {
            foreach (var prop in typeof(PermissionDefaultValue).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    var policyName = propertyValue.ToString();
                    if (!string.IsNullOrEmpty(policyName))
                    {
                        options.AddPolicy(policyName!, policy => policy.RequireClaim(policyName!, PermissionClaimTypes.Permission));
                    }
                }
            }
            //options.AddPolicy(Permission.Setting.View, policy => policy.RequireClaim(ApplicationClaimTypes.Permission));
        }


    }
}
