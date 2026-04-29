using Maanfee.JsInterop;

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
                var cult = LanguageManager.GetCultureCode(DefaultLanguage);

                LayoutSettings LayoutSettings = CreateDefaultConfiguration(cult.CultureCode, cult.IsRTL);
                await LocalStorage.SetAsync<LayoutSettings>(ConfigurationStoredName, LayoutSettings);

                SharedLayoutSettings.IsDarkMode = LayoutSettings.IsDarkMode;
                SharedLayoutSettings.IsRTL = LayoutSettings.IsRTL;
                SharedLayoutSettings.CurrentVersion = LayoutSettings.CurrentVersion;
                SharedLayoutSettings.IsFullscreenMode = LayoutSettings.IsFullscreenMode;
                SharedLayoutSettings.CultureCode = LayoutSettings.CultureCode;
                SharedLayoutSettings.Theme = LayoutSettings.Theme;
                SharedLayoutSettings.SelectedFont = LayoutSettings.SelectedFont;
                SharedLayoutSettings.RunRandomColor = LayoutSettings.RunRandomColor;
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
                SharedLayoutSettings.RunRandomColor = StoredConfiguration.RunRandomColor;
            }

            if (SharedLayoutSettings.RunRandomColor)
            {
                var ran = new Random().Next(0, MaanfeeThemeColors.SupportedThemeColors.Count);
                MaanfeeThemeColors.PrimaryColor = MaanfeeThemeColors.SupportedThemeColors[ran];

                SharedLayoutSettings.Theme = MaanfeeTheme.ThemeBuilder(MaanfeeThemeColors.PrimaryColor, MaanfeeTheme.DefaultFont().FontName);
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
                RunRandomColor = SharedLayoutSettings.RunRandomColor,
            });
        }

        private LayoutSettings CreateDefaultConfiguration(string DefaultLanguageCode, bool IsRTL)
        {
            return new LayoutSettings
            {
                IsDarkMode = false,
                IsRTL = IsRTL,
                CurrentVersion = string.Empty,
                IsFullscreenMode = false,
                CultureCode = DefaultLanguageCode,
                Theme = MaanfeeTheme.ThemeBuilder(MaanfeeThemeColors.PrimaryColor, MaanfeeTheme.DefaultFont().FontName),
                SelectedFont = MaanfeeTheme.DefaultFont(),
                RunRandomColor = true,
            };
        }

    }
}
