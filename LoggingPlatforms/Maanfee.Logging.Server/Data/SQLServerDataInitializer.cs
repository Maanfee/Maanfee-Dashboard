using Maanfee.Dashboard.Core;
using Maanfee.Logging.Domain;
using Maanfee.Logging.Domain.DAL;

namespace Maanfee.Logging.Server.Data
{
    public static class SQLServerDataInitializer
    {
        public static void Initialize(_BaseContext_SQLServer context)
        {
            AddOrUpdateData(context);
        }

        private static void AddOrUpdateData(_BaseContext_SQLServer context)
        {
            if (!context.LoggingLevels.Any())
            {
                var LoggingLevel = new List<LoggingLevel>
                    {
                        new LoggingLevel{Title = "Information".TrimStringAndCheckPersianSpecialLetter()},
                        new LoggingLevel{Title = "Debug".TrimStringAndCheckPersianSpecialLetter()},
                        new LoggingLevel{Title = "Error".TrimStringAndCheckPersianSpecialLetter()},
                        new LoggingLevel{Title = "Warning".TrimStringAndCheckPersianSpecialLetter()},
                    };
                LoggingLevel.ForEach(s => context.LoggingLevels.Add(s));

                context.SaveChanges();
            }

            if (!context.LoggingPlatforms.Any())
            {
                var LoggingPlatform = new List<LoggingPlatform>
                    {
                        new LoggingPlatform { Title = "Client".TrimStringAndCheckPersianSpecialLetter() },
                        new LoggingPlatform { Title = "Server".TrimStringAndCheckPersianSpecialLetter() },
                        new LoggingPlatform { Title = "System".TrimStringAndCheckPersianSpecialLetter() },
                    };
                LoggingPlatform.ForEach(s => context.LoggingPlatforms.Add(s));

                context.SaveChanges();
            }
        }

    }
}