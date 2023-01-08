using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
	public class DropDownReleaseTypeViewModel
	{
		public int Id { get; set; }

		[Display(Name = nameof(DashboardResource.StringTitle), ResourceType = typeof(DashboardResource))]
		public string Title { get; set; }
	}
}
