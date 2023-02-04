using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
	public class FilterRoleViewModel
	{
		[Display(Name = nameof(DashboardResource.StringRole), ResourceType = typeof(DashboardResource))]
		public virtual string Role { get; set; }
	}
}
