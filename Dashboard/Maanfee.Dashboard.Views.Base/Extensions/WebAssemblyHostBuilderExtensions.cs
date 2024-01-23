using Maanfee.Dashboard.Views.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System.Linq;
using System.Reflection;

namespace Maanfee.Dashboard.Views.Base.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
        {
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

			return builder;
        }

        private static void RegisterPermissionClaims(AuthorizationOptions options)
        {
            foreach (var prop in typeof(PermissionDefaultValue).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(propertyValue.ToString(), PermissionClaimTypes.Permission));
                }
            }

            //options.AddPolicy(Permission.Setting.View, policy => policy.RequireClaim(ApplicationClaimTypes.Permission));
        }

    }
}
