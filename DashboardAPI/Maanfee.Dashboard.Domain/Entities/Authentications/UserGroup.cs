using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class UserGroup
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // ******************************************************************

        [Display(Name = nameof(DashboardResource.StringGroup), ResourceType = typeof(DashboardResource))]
        public int IdGroup { get; set; }

        [ForeignKey("IdGroup")]
        public virtual Group? Group { get; set; }

        // ******************************************************************

        [Display(Name = nameof(DashboardResource.StringUser), ResourceType = typeof(DashboardResource))]
        public string? IdApplicationUser { get; set; }

        [ForeignKey("IdApplicationUser")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        // ******************************************************************

    }
}
