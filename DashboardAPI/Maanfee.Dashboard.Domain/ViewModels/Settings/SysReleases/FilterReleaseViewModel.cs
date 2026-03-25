using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
	public class FilterReleaseViewModel
	{
		[Display(Name = nameof(DashboardResource.StringVersion), ResourceType = typeof(DashboardResource))]
		[StringLength(10, MinimumLength = 1, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
		public string? Version { get; set; }
	}
}
