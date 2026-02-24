using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maanfee.Dashboard.Domain.Entities
{
    public partial class Department 
    {
        public Department()
        {
            this.Department1 = new HashSet<Department>();
            this.UserDepartments = new List<UserDepartment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringDepartment), ResourceType = typeof(DashboardResource))]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Title { get; set; }

        // ===============================================

        [Display(Name = nameof(DashboardResource.StringParent), ResourceType = typeof(DashboardResource))]
        public Nullable<int> IdParent { get; set; }

        [ForeignKey("IdParent")]
        public virtual Department? Department2 { get; set; }

        // ===============================================

        public virtual HashSet<Department> Department1 { get; set; }

        public virtual ICollection<UserDepartment> UserDepartments { get; set; }

    }
}
