using Maanfee.Dashboard.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DropDownDepartmentViewModel
    {
        public int Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringDepartment), ResourceType = typeof(DashboardResource))]
        [StringLength(50, MinimumLength = 0, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Title { get; set; }

        [Display(Name = nameof(DashboardResource.StringParent), ResourceType = typeof(DashboardResource))]
        public Nullable<int> IdParent { get; set; } 
    }
}
