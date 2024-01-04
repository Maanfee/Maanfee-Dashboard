using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maanfee.Logging.Domain
{
    public class GetLogInfo
    {
        [Display(Name = "#")]
        public virtual int RowNum { get; set; }

        public virtual string Id { get; set; }

        public string Message { get; set; }

        public DateTime LogDate { get; set; }

        public int IdLoggingLevel { get; set; }

        public string LoggingLevelTitle { get; set; }

        public int IdLoggingPlatform { get; set; }

        public string LoggingPlatformTitle { get; set; }
    }
}
