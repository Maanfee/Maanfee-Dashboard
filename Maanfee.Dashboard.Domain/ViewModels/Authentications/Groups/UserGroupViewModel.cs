using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class UserGroupViewModel
    {
        [Display(Name = nameof(DashboardResource.StringGroup), ResourceType = typeof(DashboardResource))]
        public int IdGroup { get; set; }

        [Display(Name = nameof(DashboardResource.StringGroup), ResourceType = typeof(DashboardResource))]
        public string GroupTitle { get; set; }

        [Display(Name = nameof(DashboardResource.StringUser), ResourceType = typeof(DashboardResource))]
        public string IdApplicationUser { get; set; }

        [Display(Name = nameof(DashboardResource.StringName), ResourceType = typeof(DashboardResource))]
        public string Name { get; set; }

        [Display(Name = nameof(DashboardResource.StringUserName), ResourceType = typeof(DashboardResource))]
        public string UserName { get; set; }
    }
}
