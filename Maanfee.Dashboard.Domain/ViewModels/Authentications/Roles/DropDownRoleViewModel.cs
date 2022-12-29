using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DropDownRoleViewModel
    {
        public virtual string Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringRole), ResourceType = typeof(DashboardResource))]
        public virtual string Name { get; set; }
    }
}
