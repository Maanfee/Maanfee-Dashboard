using System.Globalization;

namespace Maanfee.Dashboard.Views.Core
{
    public static class LanguageManager
    {
        public enum SupportedCountry
        {
            Iran, US,
        }

        public static string GetCultureCode(SupportedCountry supportedCountry) => supportedCountry switch
        {
            SupportedCountry.Iran => "fa-IR",
            SupportedCountry.US => "en-US",
            _ => "en-US"
        };

        public static List<CultureInfo> GetSupportedLanguages()
        {
            return Enum.GetValues<SupportedCountry>()
                .Select(country => new CultureInfo(GetCultureCode(country)))
                .ToList();
        }
    }
}
