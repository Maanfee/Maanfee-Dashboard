using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class Permission
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Display(Name = nameof(DashboardResource.StringTitle), ResourceType = typeof(DashboardResource))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Title { get; set; } = string.Empty;

        [Display(Name = nameof(DashboardResource.StringDisplayTitle), ResourceType = typeof(DashboardResource))]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string DisplayTitle { get; set; } = string.Empty;

        [Display(Name = nameof(DashboardResource.StringName), ResourceType = typeof(DashboardResource))]
        [StringLength(100, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = nameof(DashboardResource.StringDescription), ResourceType = typeof(DashboardResource))]
        [StringLength(500, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Description { get; set; }

        // ===============================================

        [Display(Name = nameof(DashboardResource.StringParent), ResourceType = typeof(DashboardResource))]
        public string? IdParent { get; set; }

        [ForeignKey("IdParent")]
        public virtual Permission? Parent { get; set; }

        // ===============================================

        // Navigation Properties
        public virtual ICollection<Permission> Children { get; set; } = new List<Permission>();
    }
}
