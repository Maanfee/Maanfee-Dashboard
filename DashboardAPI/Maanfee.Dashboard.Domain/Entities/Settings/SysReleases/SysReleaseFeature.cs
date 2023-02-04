using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class SysReleaseFeature
    {
        [Key]
        public string Id { get; set; }

		[Display(Name = nameof(DashboardResource.StringComment), ResourceType = typeof(DashboardResource))]
		[StringLength(500, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
		[Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
		public string Comment { get; set; }

		// ===============================================

		[Display(Name = nameof(DashboardResource.StringRelease), ResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string IdSysRelease { get; set; }

        [ForeignKey("IdSysRelease")]
        public virtual SysRelease SysRelease { get; set; }

        // ===============================================

        [Display(Name = nameof(DashboardResource.StringReleaseNoteType), ResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public int IdSysReleaseType { get; set; }

        [ForeignKey("IdSysReleaseType")]
        public virtual SysReleaseType SysReleaseType { get; set; }

        // ===============================================
    }
}
