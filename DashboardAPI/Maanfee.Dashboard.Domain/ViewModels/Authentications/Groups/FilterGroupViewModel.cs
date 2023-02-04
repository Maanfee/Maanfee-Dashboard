using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class FilterGroupViewModel
    {
        [Display(Name = nameof(DashboardResource.StringGroup), ResourceType = typeof(DashboardResource))]
        [StringLength(150, MinimumLength = 0, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Title { get; set; }
    }
}
