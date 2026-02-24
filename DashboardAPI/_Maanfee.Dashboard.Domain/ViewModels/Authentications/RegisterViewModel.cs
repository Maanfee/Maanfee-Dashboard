using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = nameof(DashboardResource.StringUserName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? UserName { get; set; }

        [Display(Name = nameof(DashboardResource.StringPersonalCode), ResourceType = typeof(DashboardResource))]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? PersonalCode { get; set; }

        [Display(Name = nameof(DashboardResource.StringPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Password { get; set; }

        [Display(Name = nameof(DashboardResource.StringConfirmPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        [Compare(nameof(Password), ErrorMessageResourceName = nameof(DashboardResource.ValidationPasswordConfirm), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Terms Service")]
        public bool IsTermsService { get; set; } = false;

        //[Display(Name = "Phrase")]
        //public string Phrase { get; set; }

    }
}
