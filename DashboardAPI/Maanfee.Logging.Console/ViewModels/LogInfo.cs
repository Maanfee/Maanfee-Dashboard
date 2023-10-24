using System;

namespace Maanfee.Logging.Console
{
    public class LogInfo
    {
		public string Platform { get; set; }

		public string Message { get; set; }

        public DateTime LogDate { get; set; }

        public LogLevel Level { get; set; }
    }
}
