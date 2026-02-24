using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class SysRelease
    {
        public SysRelease()
        {
            this.SysReleaseFeatures = new List<SysReleaseFeature>();
        }

        [Key]
        public string Id { get; set; } 

        [Display(Name = nameof(DashboardResource.StringReleasedVersions), ResourceType = typeof(DashboardResource))]
		[StringLength(10, MinimumLength = 1, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
		[Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Version { get; set; }

        //[Display(Name = nameof(DashboardResource.StringFramework), ResourceType = typeof(DashboardResource))]
        //[StringLength(20, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        //[Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        //public string Framework { get; set; }

        //[Display(Name = nameof(DashboardResource.StringDatabase), ResourceType = typeof(DashboardResource))]
        //[StringLength(20, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        //[Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        //public string DatabaseStatus { get; set; }

        [Display(Name = nameof(DashboardResource.StringDate), ResourceType = typeof(DashboardResource))]
        public Nullable<DateTime> ReleaseDate { get; set; }

        [Display(Name = nameof(DashboardResource.StringIsActive), ResourceType = typeof(DashboardResource))]
        public bool IsActive { get; set; }

        public virtual ICollection<SysReleaseFeature> SysReleaseFeatures { get; set; }
    }
}
