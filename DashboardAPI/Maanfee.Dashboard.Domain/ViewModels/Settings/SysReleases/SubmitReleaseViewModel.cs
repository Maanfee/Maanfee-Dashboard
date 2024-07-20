using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class SubmitReleaseViewModel
	{
		[Key]
		public string Id { get; set; }

		[Display(Name = nameof(DashboardResource.StringReleasedVersions), ResourceType = typeof(DashboardResource))]
		[StringLength(10, MinimumLength = 1, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
		[Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
		public string Version { get; set; }

		[Display(Name = nameof(DashboardResource.StringDate), ResourceType = typeof(DashboardResource))]
		[Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
		public Nullable<DateTime> ReleaseDate { get; set; }

        [Display(Name = nameof(DashboardResource.StringIsActive), ResourceType = typeof(DashboardResource))]
        public bool IsActive { get; set; }

		public List<SysReleaseFeatureViewModel> SysReleaseFeatureViewModels { get; set; } = new();
	}
}
