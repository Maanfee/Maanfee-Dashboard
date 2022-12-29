using Maanfee.Dashboard.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Views.Base
{
    public static class PermissionDefaultValue
    {
        //public static KeyValuePair<string, string> View =
        //   new KeyValuePair<string, string>("Permission.Settings.View", AppResource.StringList);

        [Display(Name = nameof(DashboardResource.StringSettings))]
        [Description("Settings Permissions")]
        public static class Setting
        {
            [Display(Name = nameof(DashboardResource.StringList))]
            public const string View = "Permission.Settings.View";
        }

        [Display(Name = nameof(DashboardResource.StringRole))]
        [Description("Roles Permissions")]
        public static class Role
        {
            [Display(Name = nameof(DashboardResource.StringList))]
            public const string View = "Permission.Roles.View";

            [Display(Name = nameof(DashboardResource.StringPermission))]
            public const string Permission = "Permission.Roles.Permission";

            [Display(Name = nameof(DashboardResource.StringCreate))]
            public const string Create = "Permission.Roles.Create";

            [Display(Name = nameof(DashboardResource.StringEdit))]
            public const string Edit = "Permission.Roles.Edit";

            [Display(Name = nameof(DashboardResource.StringDelete))]
            public const string Delete = "Permission.Roles.Delete";

            [Display(Name = nameof(DashboardResource.StringDetails))]
            public const string Details = "Permission.Roles.Details";
        }

        [Display(Name = nameof(DashboardResource.StringGroup))]
        [Description("Group Permissions")]
        public static class Group
        {
            [Display(Name = nameof(DashboardResource.StringList))]
            public const string View = "Permission.Group.View";
        }

        [Display(Name = nameof(DashboardResource.StringDepartment))]
        [Description("Departments Permissions")]
        public static class Department
        {
            [Display(Name = nameof(DashboardResource.StringList))]
            public const string View = "Permission.Departments.View";

            [Display(Name = nameof(DashboardResource.StringCreate))]
            public const string Create = "Permission.Departments.Create";

            [Display(Name = nameof(DashboardResource.StringDelete))]
            public const string Delete = "Permission.Departments.Delete";
        }

        [Display(Name = nameof(DashboardResource.StringUser))]
        [Description("Users Permissions")]
        public static class User
        {
            [Display(Name = nameof(DashboardResource.StringList))]
            public const string View = "Permission.Users.View";

            [Display(Name = nameof(DashboardResource.StringCreate))]
            public const string Create = "Permission.Users.Create";

            [Display(Name = nameof(DashboardResource.StringEdit))]
            public const string Edit = "Permission.Users.Edit";

            [Display(Name = nameof(DashboardResource.StringDelete))]
            public const string Delete = "Permission.Users.Delete";

            [Display(Name = nameof(DashboardResource.StringDetails))]
            public const string Details = "Permission.Users.Details";
        }

        [Display(Name = nameof(DashboardResource.StringRelease))]
        [Description("System Release")]
        public static class SystemRelease
        {
            [Display(Name = nameof(DashboardResource.StringList))]
            public const string View = "Permission.SystemRelease.View";

            [Display(Name = nameof(DashboardResource.StringCreate))]
            public const string Create = "Permission.SystemRelease.Create";

            [Display(Name = nameof(DashboardResource.StringEdit))]
            public const string Edit = "Permission.SystemRelease.Edit";

            [Display(Name = nameof(DashboardResource.StringDelete))]
            public const string Delete = "Permission.SystemRelease.Delete";

            [Display(Name = nameof(DashboardResource.StringDetails))]
            public const string Details = "Permission.SystemRelease.Details";
        }

        //[Display(Name = nameof(AppResource.StringDelegationSuccessor))]
        //[Description("Delegation Successor")]
        //public static class DelegationSuccessor
        //{
        //    [Display(Name = nameof(DashboardResource.StringList))]
        //    public const string View = "Permission.DelegationSuccessor.View";
        //}

        //[Display(Name = nameof(AppResource.StringPublicRequests))]
        //[Description("Public Requests")]
        //public static class PublicRequests
        //{
        //    [Display(Name = nameof(DashboardResource.StringList))]
        //    public const string View = "Permission.PublicRequests.View";

        //    [Display(Name = nameof(DashboardResource.StringCreate))]
        //    public const string Create = "Permission.PublicRequests.Create";

        //    [Display(Name = nameof(DashboardResource.StringEdit))]
        //    public const string Edit = "Permission.PublicRequests.Edit";

        //    [Display(Name = nameof(DashboardResource.StringDelete))]
        //    public const string Delete = "Permission.PublicRequests.Delete";

        //    [Display(Name = nameof(DashboardResource.StringDetails))]
        //    public const string AdvancedFilter = "Permission.PublicRequests.AdvancedFilter";
        //}

        //[Display(Name = nameof(AppResource.StringReports))]
        //[Description("Reports")]
        //public static class Reports
        //{
        //    [Display(Name = nameof(AppResource.StringList))]
        //    public const string View = "Permission.Reports.View";
        //}

        [Display(Name = nameof(DashboardResource.StringSwagger))]
        [Description("Swagger")]
        public static class Swaggers
        {
            [Display(Name = nameof(DashboardResource.StringList))]
            public const string View = "Permission.Swaggers.View";
        }
    }
}
