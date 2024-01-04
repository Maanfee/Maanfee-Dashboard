using Maanfee.Logging.Domain;

namespace Maanfee.Logging.Domain
{
    public static class LoggingLevelDefaultValue
    {
        public const int Information = 1;
        public const int Debug = 2;
        public const int Error = 3;
        public const int Warning = 4;

        //public static List<LoggingLevel> GetLoggingLevel()
        //{
        //    return new List<LoggingLevel>
        //            {
        //                new LoggingLevel{Id = 1, Title = "Information"},
        //                new LoggingLevel{Id = 2, Title = "Debug"},
        //                new LoggingLevel{Id = 3, Title = "Error"},
        //                new LoggingLevel{Id = 4, Title = "Warning"},
        //            };
        //}
    }
}
