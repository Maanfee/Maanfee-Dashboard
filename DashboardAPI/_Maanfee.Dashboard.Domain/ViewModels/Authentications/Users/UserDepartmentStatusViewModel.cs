using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class UserDepartmentStatusViewModel
    {
        [Display(Name = nameof(DashboardResource.StringDepartment), ResourceType = typeof(DashboardResource))]
        public string? DepartmentUserTitle { get; set; }

        // ***************************

        public int DepartmentParentId { get; set; }

        [Display(Name = nameof(DashboardResource.StringDepartment), ResourceType = typeof(DashboardResource))]
        public string? DepartmentParentTitle { get; set; }

        [Display(Name = nameof(DashboardResource.StringUser), ResourceType = typeof(DashboardResource))]
        public string? UserParentName { get; set; }

        [Display(Name = nameof(DashboardResource.StringUserName), ResourceType = typeof(DashboardResource))]
        public string? UserParentUserName { get; set; }

        public bool CanSignaturePermit { get; set; }

        // برای کامبوباکس کاربران
        public DropDownUserViewModel? ReceiverUser { get; set; }
    }
}
