using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DropDownPermissionViewModel : DropDownViewModel<string>
    {
        [Display(Name = nameof(DashboardResource.StringName), ResourceType = typeof(DashboardResource))]
        public string? Name { get; set; }

        [Display(Name = nameof(DashboardResource.StringParent), ResourceType = typeof(DashboardResource))]
        public string? IdParent { get; set; }
    }
}
