using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DropDownDepartmentViewModel : DropDownViewModel<int>
    {
        [Display(Name = nameof(DashboardResource.StringParent), ResourceType = typeof(DashboardResource))]
        public Nullable<int> IdParent { get; set; }
    }
}
