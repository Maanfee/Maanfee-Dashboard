using Maanfee.Dashboard.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class SubmitUserViewModel
    { 
        public string Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringUserName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 1, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string UserName { get; set; }

        [Display(Name = nameof(DashboardResource.StringPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Password { get; set; }

        [Display(Name = nameof(DashboardResource.StringAvatar), ResourceType = typeof(DashboardResource))]
        public byte[] Avatar { get; set; }

        [Display(Name = nameof(DashboardResource.StringPersonalCode), ResourceType = typeof(DashboardResource))]
        [StringLength(10, MinimumLength = 1, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string PersonalCode { get; set; }

        // **************************************************

        [Display(Name = nameof(DashboardResource.StringGender), ResourceType = typeof(DashboardResource))]
        public DropDownGenderViewModel Gender { get; set; }

        [Display(Name = nameof(DashboardResource.StringRole), ResourceType = typeof(DashboardResource))]
        public DropDownRoleViewModel Role { get; set; }

        // **************************************************

        public int? IdDepartment { get; set; }

        public IEnumerable<int?> DepartmentPersonalValues { get; set; } = new List<int?>();

        public IEnumerable<int?> DepartmentManagementValues { get; set; } = new List<int?>();

        //[Display(Name = nameof(DashboardResource.StringDepartment), ResourceType = typeof(DashboardResource))]
        //public DropDownDepartmentViewModel Department { get; set; }

        // **************************************************

        public int? IdGroup { get; set; }

        public IEnumerable<int?> GroupValues { get; set; } = new List<int?>();

        // **************************************************

        [Display(Name = nameof(DashboardResource.StringFirstName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string FirstName { get; set; }

        [Display(Name = nameof(DashboardResource.StringLastName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string LastName { get; set; }

        [Display(Name = nameof(DashboardResource.StringFatherName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string FatherName { get; set; }

        [Display(Name = nameof(DashboardResource.StringNationalCode), ResourceType = typeof(DashboardResource))]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceName = nameof(DashboardResource.ValidationPositiveNumbers), ErrorMessageResourceType = typeof(DashboardResource))]
        public string NationalCode { get; set; }

        [Display(Name = nameof(DashboardResource.StringPhoneNumber), ResourceType = typeof(DashboardResource))]
        [StringLength(11, MinimumLength = 11, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceName = nameof(DashboardResource.ValidationPositiveNumbers), ErrorMessageResourceType = typeof(DashboardResource))]
        public virtual string PhoneNumber { get; set; }
    }
}
