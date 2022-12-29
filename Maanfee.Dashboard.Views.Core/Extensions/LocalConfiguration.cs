using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Views.Core.DefaultValues;
using Maanfee.Dashboard.Views.Core.Services;
using Maanfee.Web.JSInterop;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Core.Extensions
{
	public class LocalConfiguration
	{
		public LocalConfiguration(LocalStorage localStorage)
		{
            LocalStorage = localStorage;			
		}

		public LocalStorage LocalStorage;

		private async Task<CultureInfo> CultureConfigurationAsync()
		{
			CultureInfo Culture;

			var CultureStorage = await LocalStorage.GetAsync<LanguageModel>(StorageDefaultValue.CultureStorage);
			if (CultureStorage != null)
			{
				Culture = new CultureInfo(CultureStorage.Code);
			}
			else
			{
				var DefaultLanguageModel = LanguageService.GetLanguage("US");

				Culture = new CultureInfo(DefaultLanguageModel.Code);
				await LocalStorage.SetAsync<LanguageModel>(StorageDefaultValue.CultureStorage, DefaultLanguageModel);
			}

			return Culture;
		}

		public async Task InitConfigurationAsync()
		{
			var ConfigurationModel = await LocalStorage.GetAsync<ConfigurationModel>(StorageDefaultValue.ConfigurationStorage);
			if (ConfigurationModel == null)
			{
				await LocalStorage.SetAsync<ConfigurationModel>(StorageDefaultValue.ConfigurationStorage, new ConfigurationModel
				{
					IsDarkMode = false,
				});
			}
		}

		// ***********************

		public async Task InitWasmCultureAsync(IServiceCollection Services)
		{
			Services.AddLocalization(options =>
			{
				options.ResourcesPath = "Resources";
			});

			var Culture = await CultureConfigurationAsync();

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
