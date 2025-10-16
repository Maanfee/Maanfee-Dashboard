using Maanfee.Dashboard.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Views.Base
{
	public static class PermissionDefaultValue
	{
		//public static KeyValuePair<string, string> View =
		//   new KeyValuePair<string, string>("Permission.Settings.View", AppResource.StringList);

		#region - Roles -

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
		}

		#endregion

		#region - Group -

		[Display(Name = nameof(DashboardResource.StringGroup))]
		[Description("Group Permissions")]
		public static class Group
		{
			[Display(Name = nameof(DashboardResource.StringList))]
			public const string View = "Permission.Group.View";
		}

		#endregion

		#region - Departments -

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

		#endregion

		#region - Users -

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
		}

		#endregion

		#region - Settings -

		[Display(Name = nameof(DashboardResource.StringSettings))]
		[Description("Settings Permissions")]
		public static class Setting
		{
			[Display(Name = nameof(DashboardResource.StringRelease))]
			public const string ReleaseManagementView = "Permission.Settings.ReleaseManagementView";

			[Display(Name = nameof(DashboardResource.StringSwagger))]
			public const string Swagger = "Permission.Settings.SwaggersView";

            [Display(Name = nameof(Maanfee.Logging.Console.Resources.Resource.StringMonitoringLogs))]
            public const string MonitorLogs = "Permission.Settings.MonitorLogs";
        }

		#endregion

	}
}
