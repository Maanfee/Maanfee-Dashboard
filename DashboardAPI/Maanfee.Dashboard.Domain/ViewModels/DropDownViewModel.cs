using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DropDownViewModel<T>
    {
        public virtual T? Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringDepartment), ResourceType = typeof(DashboardResource))]
        [StringLength(50, MinimumLength = 0, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public virtual string? Title { get; set; }
    }
}
