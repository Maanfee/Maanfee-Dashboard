using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maanfee.Dashboard.Domain
{
    public class UserDepartment
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // ******************************************************************

        [Display(Name = nameof(DashboardResource.StringDepartment), ResourceType = typeof(DashboardResource))]
        public int IdDepartment { get; set; }

        [ForeignKey("IdDepartment")]
        public virtual Department? Department { get; set; }

        // ******************************************************************

        [Display(Name = nameof(DashboardResource.StringUser), ResourceType = typeof(DashboardResource))]
        public string? IdApplicationUser { get; set; }

        [ForeignKey("IdApplicationUser")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        // ******************************************************************

    }
}
