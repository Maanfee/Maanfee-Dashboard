using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class SubmitPermissionViewModel
    {
        public string? Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringTitle), ResourceType = typeof(DashboardResource))]
        [StringLength(150, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Title { get; set; }

        [Display(Name = nameof(DashboardResource.StringTitle), ResourceType = typeof(DashboardResource))]
        [StringLength(150, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? DisplayTitle { get; set; }

        [Display(Name = nameof(DashboardResource.StringName), ResourceType = typeof(DashboardResource))]
        [StringLength(100, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = nameof(DashboardResource.StringDescription), ResourceType = typeof(DashboardResource))]
        [StringLength(500, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Description { get; set; }

        public DropDownPermissionViewModel? Parent { get; set; } = new();
    }
}
