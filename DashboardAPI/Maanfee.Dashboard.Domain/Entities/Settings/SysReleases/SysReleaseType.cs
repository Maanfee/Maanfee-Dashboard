using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class SysReleaseType
    {
        public SysReleaseType()
        {
            this.SysReleaseFeatures = new List<SysReleaseFeature>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringTitle), ResourceType = typeof(DashboardResource))]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Title { get; set; }

        public virtual ICollection<SysReleaseFeature> SysReleaseFeatures { get; set; }
    }
}
