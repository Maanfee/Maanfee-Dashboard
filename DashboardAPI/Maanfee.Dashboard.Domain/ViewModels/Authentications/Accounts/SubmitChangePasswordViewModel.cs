using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class SubmitChangePasswordViewModel
    {
        [Display(Name = nameof(DashboardResource.StringCurrentPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]

        public string CurrentPassword { get; set; }

        [Display(Name = nameof(DashboardResource.StringNewPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]

        public string NewPassword { get; set; }

        [Display(Name = nameof(DashboardResource.StringConfirmNewPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]

        [Compare(nameof(NewPassword), ErrorMessageResourceName = nameof(DashboardResource.ValidationPasswordConfirm), ErrorMessageResourceType = typeof(DashboardResource))]
        public string ConfirmNewPassword { get; set; }
    }
}
