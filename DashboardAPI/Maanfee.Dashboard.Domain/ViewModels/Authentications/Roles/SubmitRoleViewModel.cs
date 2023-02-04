using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class SubmitRoleViewModel
    {
        public virtual string Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringRole), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = "ValidationStringLength", ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public virtual string Role { get; set; }
    }
}
