using Maanfee.Dashboard.Domain.DAL;
using System.Collections.Generic;
using System.Linq;
using Maanfee.Dashboard.Domain.DefaultValues;
using Maanfee.Dashboard.Domain.Entities;

namespace Maanfee.Dashboard.Server
{
    public static class SQLiteDataInitializer
    {
        public static void Initialize(_BaseContext_SQLite context)
        {
            AddOrUpdateSettings(context);
        }

        private static void AddOrUpdateSettings(_BaseContext_SQLite context)
        {
            if (!context.SystemReleaseNoteTypes.Any())
            {
                var SystemReleaseNoteType = new List<SystemReleaseNoteType>
                {
                    new SystemReleaseNoteType
                    {
                        Title = "امکانات (Features)",
                    },
                    new SystemReleaseNoteType
                    {
                        Title = "رفع اشکال (Bug Fixes)",
                    },
                };
                SystemReleaseNoteType.ForEach(s => context.SystemReleaseNoteTypes.Add(s));

                context.SaveChanges();
            }

        }

    }
}
