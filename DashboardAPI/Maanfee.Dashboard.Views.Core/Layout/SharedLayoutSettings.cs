using MudBlazor;

namespace Maanfee.Dashboard.Views.Core
{
    public static class SharedLayoutSettings
    {
        public static bool IsRTL { get; set; }

        public static bool IsDarkMode { get; set; }

        public static string? CurrentVersion { get; set; }

        public static bool IsFullscreenMode { get; set; }

        public static string CultureCode { get; set; } = LanguageManager.GetCultureCode(LanguageManager.SupportedCountry.US);

        public static MudTheme Theme { get; set; } = new MudTheme();

        public static Font? SelectedFont { get; set; } 

    }
}

