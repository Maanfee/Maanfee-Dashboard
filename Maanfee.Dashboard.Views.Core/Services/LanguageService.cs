using Maanfee.Dashboard.Core;
using System.Collections.Generic;
using System.Linq;

namespace Maanfee.Dashboard.Views.Core.Services
{
    public static class LanguageService
    {
        public static List<LanguageModel> GetSupportedLanguages()
        {
            return new List<LanguageModel>
            {
                new LanguageModel
                {
                    Code = "fa-IR",
                    Country = "Iran",
                    Language = "Persian",
                    IsRTL = true,
                },
                new LanguageModel
                {
                    Code = "en-US",
                    Country = "US",
                    Language = "English",
                    IsRTL = false,
                },
            };
        }

        public static LanguageModel GetLanguage(string Country)
        {
            return GetSupportedLanguages().FirstOrDefault(x => x.Country == Country);
        }

        public static string ToDisplayString(LanguageModel LanguageModel)
        {
            return string.Format("{0} ({1})", LanguageModel.Language, LanguageModel.Country);
        }
    }
}
