using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DropDownGroupViewModel
    {
        public int Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringTitle), ResourceType = typeof(DashboardResource))]
        [StringLength(150, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Title { get; set; }
    }
}
