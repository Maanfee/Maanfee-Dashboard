namespace Maanfee.Logging.Domain
{
    public static class LoggingPlatformDefaultValue
    {
        public static int Client = 1;
        public static int Server = 2;
        public static int System = 3;

        public static string GetLoggingPlatformTitle(int Id)
        {
            switch (Id)
            {
                case 1:
                    return "Client";
                case 2:
                    return "Server";
                case 3:
                    return "System";
                default:
                    return "Unknown Platform";
            }
        }
    }
}
