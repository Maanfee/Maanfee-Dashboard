using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DropDownUserViewModel
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        [Display(Name = nameof(DashboardResource.StringName), ResourceType = typeof(DashboardResource))]
        public virtual string? Name { get; set; }

        [Display(Name = nameof(DashboardResource.StringUserName), ResourceType = typeof(DashboardResource))]
        public virtual string? UserName { get; set; }
    }
}
