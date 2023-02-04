using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class SubmitSysReleaseFeatureViewModel
    {
        [Display(Name = nameof(DashboardResource.StringComment), ResourceType = typeof(DashboardResource))]
        [StringLength(500, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Comment { get; set; }

		[Display(Name = nameof(DashboardResource.StringReleaseType), ResourceType = typeof(DashboardResource))]
		[Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
		public DropDownReleaseTypeViewModel ReleaseType { get; set; }

	}
}
