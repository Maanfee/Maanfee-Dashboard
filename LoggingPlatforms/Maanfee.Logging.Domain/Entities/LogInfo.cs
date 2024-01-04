using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maanfee.Logging.Domain
{
	public class LogInfo
	{
		[Key]
		public string Id { get; set; }

        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Message { get; set; }

        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public DateTime LogDate { get; set; }

		// ===============================================

		//[Display(Name = nameof(AppResource.StringAccountGroup), ResourceType = typeof(AppResource))]
		[Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
		public int IdLoggingLevel { get; set; }

		[ForeignKey(nameof(IdLoggingLevel))]
		public virtual LoggingLevel LoggingLevel { get; set; }

		// ===============================================

		//[Display(Name = nameof(AppResource.StringAccountGroup), ResourceType = typeof(AppResource))]
		[Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
		public int IdLoggingPlatform { get; set; }

		[ForeignKey(nameof(IdLoggingPlatform))]
		public virtual LoggingPlatform LoggingPlatform { get; set; }

		// ===============================================
	}
}
