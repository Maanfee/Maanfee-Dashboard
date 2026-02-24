using MudBlazor;

namespace Maanfee.Dashboard.Views.Core
{
    public class LayoutSettings
    {
        public bool IsRTL { get; set; }

        public bool IsDarkMode { get; set; }

        public string? CurrentVersion { get; set; }

        public bool IsFullscreenMode { get; set; }

        public string CultureCode { get; set; } = LanguageManager.GetCultureCode(LanguageManager.SupportedCountry.US);

        public MudTheme Theme { get; set; } = new MudTheme();

        public Font? SelectedFont { get; set; }
    }
}
