using Maanfee.Dashboard.Resources;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.UserDepartments = new List<UserDepartment>();
            this.UserGroups = new List<UserGroup>();
		}

        // نام کوچک
        [Display(Name = nameof(DashboardResource.StringFirstName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? FirstName { get; set; }

        // نام خانوادگی
        [Display(Name = nameof(DashboardResource.StringLastName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? LastName { get; set; }

        // نام
        [Display(Name = nameof(DashboardResource.StringName), ResourceType = typeof(DashboardResource))]
        [StringLength(60, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Name { get; set; }

        [Display(Name = nameof(DashboardResource.StringFatherName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? FatherName { get; set; }

        [Display(Name = nameof(DashboardResource.StringNationalCode), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 10, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceName = nameof(DashboardResource.ValidationPositiveNumbers), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? NationalCode { get; set; }

        [Display(Name = nameof(DashboardResource.StringPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? Password { get; set; }

        [Display(Name = nameof(DashboardResource.StringPersonalCode), ResourceType = typeof(DashboardResource))]
        [StringLength(10, MinimumLength = 4, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string? PersonalCode { get; set; }

        [Display(Name = nameof(DashboardResource.StringAvatar), ResourceType = typeof(DashboardResource))]
        //[StringLength(int.MaxValue, MinimumLength = 0, ErrorMessageResourceName = "ValidationStringLength", ErrorMessageResourceType = typeof(AppResourceEn))]
        //[Column(TypeName = "ntext")]
        //[DataType(DataType.MultilineText)]
        public byte[]? Avatar { get; set; }

        [Display(Name = nameof(DashboardResource.StringIsActive), ResourceType = typeof(DashboardResource))]
        public bool IsActive { get; set; }

        // ******************************************************************

        [Display(Name = nameof(DashboardResource.StringGender), ResourceType = typeof(DashboardResource))]
        public Nullable<int> IdGender { get; set; }

        [ForeignKey("IdGender")]
        public virtual Gender? Gender { get; set; }

        // ******************************************************************

        public virtual ICollection<UserDepartment> UserDepartments { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
	}
}
