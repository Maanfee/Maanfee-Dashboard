using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class FilterGroupViewModel : _BaseViewModel
    {
        [Display(Name = nameof(DashboardResource.StringGroup), ResourceType = typeof(DashboardResource))]
        public string Title { get; set; }
    }
}
