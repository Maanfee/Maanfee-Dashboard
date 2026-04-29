using System.Globalization;

namespace Maanfee.Dashboard.Views.Core
{
    public static class LanguageManager
    {
        public enum SupportedCountry
        {
            Iran, US,
        } 

        public static (string CultureCode, bool IsRTL) GetCultureCode(SupportedCountry supportedCountry) => supportedCountry switch
        {
            SupportedCountry.Iran => ("fa-IR", true),
            SupportedCountry.US => ("en-US", false),
            _ => ("en-US", false)
        };

        public static List<CultureInfo> GetSupportedLanguages()
        {
            return Enum.GetValues<SupportedCountry>()
                .Select(country => new CultureInfo(GetCultureCode(country).CultureCode))
                .ToList();
        }
    }
}
