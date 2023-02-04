using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DropDownGenderViewModel
    {
        public int Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringGender), ResourceType = typeof(DashboardResource))]
        public string Sex { get; set; }
    }
}
