using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Resources;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class GetUserViewModel
    {
        [Display(Name = "#")]
        public virtual int RowNum { get; set; }

        public virtual string Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringUserName), ResourceType = typeof(DashboardResource))]
        public virtual string UserName { get; set; }

        [Display(Name = nameof(DashboardResource.StringRole), ResourceType = typeof(DashboardResource))]
        public virtual string RoleName { get; set; }

        [Display(Name = nameof(DashboardResource.StringPersonalCode), ResourceType = typeof(DashboardResource))]
        public string PersonalCode { get; set; }

        [Display(Name = nameof(DashboardResource.StringAvatar), ResourceType = typeof(DashboardResource))]
        public byte[] Avatar { get; set; }

        // **************************************************

        [Display(Name = nameof(DashboardResource.StringGender), ResourceType = typeof(DashboardResource))]
        public Gender Gender { get; set; }

        [Display(Name = nameof(DashboardResource.StringRole), ResourceType = typeof(DashboardResource))]
        public IdentityRole Role { get; set; }

        [Display(Name = nameof(DashboardResource.StringPersonalPost), ResourceType = typeof(DashboardResource))]
        public virtual string UserDepartmentsPersonalTitle { get; set; }

        [Display(Name = nameof(DashboardResource.StringManagementPost), ResourceType = typeof(DashboardResource))]
        public virtual string UserDepartmentsManagementTitle { get; set; }

        [Display(Name = nameof(DashboardResource.StringGroup), ResourceType = typeof(DashboardResource))]
        public virtual string UserGroupTitle { get; set; }

        // **************************************************

        [Display(Name = nameof(DashboardResource.StringPassword), ResourceType = typeof(DashboardResource))]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Password { get; set; }

        [Display(Name = nameof(DashboardResource.StringFirstName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string FirstName { get; set; }

        [Display(Name = nameof(DashboardResource.StringLastName), ResourceType = typeof(DashboardResource))]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string LastName { get; set; }

        [Display(Name = nameof(DashboardResource.StringName), ResourceType = typeof(DashboardResource))]
        [StringLength(60, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Name { get; set; }

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
