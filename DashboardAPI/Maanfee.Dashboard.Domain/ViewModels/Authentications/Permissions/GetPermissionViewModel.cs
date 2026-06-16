using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class GetPermissionViewModel
    {
        [Display(Name = "#")]
        public virtual int RowNum { get; set; }

        public string? Id { get; set; }

        //[Display(Name = nameof(DashboardResource.StringPermissionName), ResourceType = typeof(DashboardResource))]
        public string? Title { get; set; }

        //[Display(Name = nameof(DashboardResource.StringDisplayName), ResourceType = typeof(DashboardResource))]
        public string? DisplayTitle { get; set; }

        [Display(Name = nameof(DashboardResource.StringDescription), ResourceType = typeof(DashboardResource))]
        public string? Description { get; set; }

        // ===============================================

        //[Display(Name = nameof(DashboardResource.StringPermissionName), ResourceType = typeof(DashboardResource))]
        public string? ParentTitle { get; set; }

        //[Display(Name = nameof(DashboardResource.StringDisplayName), ResourceType = typeof(DashboardResource))]
        public string? ParentDisplayTitle { get; set; }

        [Display(Name = nameof(DashboardResource.StringIsActive), ResourceType = typeof(DashboardResource))]
        public bool IsActive { get; set; } = true;
    }
}
