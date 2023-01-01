using Maanfee.Dashboard.Core;
using Maanfee.Dashboard.Views.Core.DefaultValues;
using Maanfee.Dashboard.Views.Core.Extensions;
using Maanfee.Web.JSInterop;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Views.Core.Shared
{
    public class SharedLayout : LayoutComponentBase 
    {
#pragma warning disable CS0108

        [Inject]
        protected LocalStorage LocalStorage { get; set; }

#pragma warning restore CS0108

        protected LanguageModel LanguageModel = new();

        protected virtual MudTheme CurrentTheme { get; set; }

        protected override async Task OnInitializedAsync()
        {
			LanguageModel = await LocalStorage.GetAsync<LanguageModel>(StorageDefaultValue.CultureStorage);
            SharedLayoutSettings.IsRTL = LanguageModel.IsRTL;

            CurrentTheme = MaanfeeTheme.ThemeBuilder(SharedLayoutSettings.IsRTL, SharedLayoutSettings.IsDarkMode);
        }

	}
}
