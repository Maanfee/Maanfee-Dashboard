using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = nameof(DashboardResource.StringUserName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 1, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? UserName { get; set; }

        [Display(Name = nameof(DashboardResource.StringPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
