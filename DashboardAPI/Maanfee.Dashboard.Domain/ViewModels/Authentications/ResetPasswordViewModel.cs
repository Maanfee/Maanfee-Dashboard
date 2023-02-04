using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Phrase Words")]
        public string Phrase { get; set; }

        [Display(Name = nameof(DashboardResource.StringPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Password { get; set; }

        [Display(Name = "Password Confirmation")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
        public string PasswordConfirm { get; set; }
    }
}
