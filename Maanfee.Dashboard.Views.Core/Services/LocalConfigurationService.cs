using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Views.Core.DefaultValues;
using Maanfee.Web.JSInterop;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public class LocalConfigurationService
    {
        public LocalConfigurationService(LocalStorage localStorage)
        {
            LocalStorage = localStorage;
        }

        public LocalStorage LocalStorage;

        private async Task<CultureInfo> CultureConfigurationAsync(string DefaultLanguage)
        {
            CultureInfo Culture;

            var CultureStorage = await LocalStorage.GetAsync<LanguageModel>(StorageDefaultValue.CultureStorage);
            if (CultureStorage != null)
            {
                Culture = new CultureInfo(CultureStorage.Code);
            }
            else
            {
                var DefaultLanguageModel = LanguageService.GetLanguage(DefaultLanguage);

                Culture = new CultureInfo(DefaultLanguageModel.Code);
                await LocalStorage.SetAsync<LanguageModel>(StorageDefaultValue.CultureStorage, DefaultLanguageModel);
            }

            return Culture;
        }

        #region - Configuration -

        public async Task GetConfigurationAsync()
        {
            var LayoutSettings = new LayoutSettings()
            {
                IsDarkMode = false,
                IsRTL = false,
                ThemeColor = "#594AE2",
            };
            var ConfigurationModel = await LocalStorage.GetAsync<LayoutSettings>(StorageDefaultValue.ConfigurationStorage);
            if (ConfigurationModel == null)
            {
                await LocalStorage.SetAsync<LayoutSettings>(StorageDefaultValue.ConfigurationStorage, LayoutSettings);
            }
            else
            {
                SharedLayoutSettings.IsDarkMode = ConfigurationModel.IsDarkMode;
                SharedLayoutSettings.IsRTL = ConfigurationModel.IsRTL;
                SharedLayoutSettings.ThemeColor = ConfigurationModel.ThemeColor;
            }
        }

        public async Task SetConfigurationAsync()
        {
            var LayoutSettings = new LayoutSettings()
            {
                IsDarkMode = SharedLayoutSettings.IsDarkMode,
                IsRTL = SharedLayoutSettings.IsRTL,
                ThemeColor = SharedLayoutSettings.ThemeColor,
            };
            await LocalStorage.SetAsync<LayoutSettings>(StorageDefaultValue.ConfigurationStorage, LayoutSettings);
        }

        #endregion

        // *********************** 

        public async Task InitWasmCultureAsync(IServiceCollection Services, LanguageService.SupportedCountry DefaultLanguage)
        {
            Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            var Culture = await CultureConfigurationAsync(LanguageService.GetCountryName(DefaultLanguage));

            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;
            CultureInfo.CurrentCulture = Culture;
            CultureInfo.CurrentUICulture = Culture;
        }

        public static void InitServerCultureAsync(IApplicationBuilder app)
        {
            //using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var LocalConfiguration = scope.ServiceProvider.GetService<LocalConfiguration>();
            //    if (LocalConfiguration != null)
            //    {
            //    }
            //}

            var Culture = new CultureInfo("fa-IR");
            //var Culture = new CultureInfo("en-US");

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(Culture),
                SupportedCultures = new List<CultureInfo> { Culture, },
                SupportedUICultures = new List<CultureInfo> { Culture, },
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                },
            });
        }

    }
}
