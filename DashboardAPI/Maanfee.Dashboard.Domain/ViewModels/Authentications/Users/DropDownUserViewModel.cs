using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DropDownUserViewModel
    {
        public virtual string Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringName), ResourceType = typeof(DashboardResource))]
        public virtual string Name { get; set; }

        [Display(Name = nameof(DashboardResource.StringUserName), ResourceType = typeof(DashboardResource))]
        public virtual string UserName { get; set; }
    }
}
