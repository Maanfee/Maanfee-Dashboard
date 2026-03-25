using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.Entities
{
    public partial class Gender
    {
        public Gender()
        {
            this.ApplicationUsers = new List<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringGender), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Sex { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
