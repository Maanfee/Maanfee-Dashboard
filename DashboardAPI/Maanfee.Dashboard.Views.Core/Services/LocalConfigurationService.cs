using Maanfee.JsInterop;
using MudBlazor;

namespace Maanfee.Dashboard.Views.Core
{
    public class LocalConfigurationService
    {
        public LocalConfigurationService(LocalStorage localStorage)
        {
            LocalStorage = localStorage;
        }

        public LocalStorage LocalStorage;
        private const string ConfigurationStoredName = "ConfigurationStorage";

        public async Task InitConfigurationAsync(LanguageManager.SupportedCountry DefaultLanguage)
        {
            var StoredConfiguration = await LocalStorage.GetAsync<LayoutSettings>(ConfigurationStoredName);
            if (StoredConfiguration == null)
            {
                LayoutSettings LayoutSettings = CreateDefaultConfiguration(LanguageManager.GetCultureCode(DefaultLanguage));
                await LocalStorage.SetAsync<LayoutSettings>(ConfigurationStoredName, LayoutSettings);

                SharedLayoutSettings.IsDarkMode = LayoutSettings.IsDarkMode;
                SharedLayoutSettings.IsRTL = LayoutSettings.IsRTL;
                SharedLayoutSettings.CurrentVersion = LayoutSettings.CurrentVersion;
                SharedLayoutSettings.IsFullscreenMode = LayoutSettings.IsFullscreenMode;
                SharedLayoutSettings.CultureCode = LayoutSettings.CultureCode;
                SharedLayoutSettings.Theme = LayoutSettings.Theme;
                SharedLayoutSettings.SelectedFont = LayoutSettings.SelectedFont;
            }
            else
            {
                SharedLayoutSettings.IsDarkMode = StoredConfiguration.IsDarkMode;
                SharedLayoutSettings.IsRTL = StoredConfiguration.IsRTL;
                SharedLayoutSettings.CurrentVersion = StoredConfiguration.CurrentVersion;
                SharedLayoutSettings.IsFullscreenMode = StoredConfiguration.IsFullscreenMode;
                SharedLayoutSettings.CultureCode = StoredConfiguration.CultureCode;
                SharedLayoutSettings.Theme = StoredConfiguration.Theme;
                SharedLayoutSettings.SelectedFont = StoredConfiguration.SelectedFont;
            }
        }

        public async Task SetConfigurationAsync()
        {
            await LocalStorage.SetAsync<LayoutSettings>(ConfigurationStoredName, new LayoutSettings()
            {
                IsDarkMode = SharedLayoutSettings.IsDarkMode,
                IsRTL = SharedLayoutSettings.IsRTL,
                CurrentVersion = SharedLayoutSettings.CurrentVersion,
                IsFullscreenMode = SharedLayoutSettings.IsFullscreenMode,
                CultureCode = SharedLayoutSettings.CultureCode,
                Theme = SharedLayoutSettings.Theme,
                SelectedFont = SharedLayoutSettings.SelectedFont,
            });
        }

        private LayoutSettings CreateDefaultConfiguration(string DefaultLanguageCode)
        {
            return new LayoutSettings
            {
                IsDarkMode = false,
                IsRTL = false,
                CurrentVersion = string.Empty,
                IsFullscreenMode = false,
                CultureCode = DefaultLanguageCode,
                Theme = MaanfeeTheme.ThemeBuilder(new PaletteLight().Primary, MaanfeeTheme.DefaultFont().FontName),
                SelectedFont = MaanfeeTheme.DefaultFont(),
            };
        }

    }
}
