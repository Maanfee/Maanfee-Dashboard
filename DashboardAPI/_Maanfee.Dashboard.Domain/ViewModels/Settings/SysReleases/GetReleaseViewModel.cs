using Maanfee.Dashboard.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
	public class GetReleaseViewModel
	{
		[Display(Name = "#")]
		public virtual int RowNum { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Display(Name = nameof(DashboardResource.StringVersion), ResourceType = typeof(DashboardResource))]
		public string? Version { get; set; }

		[Display(Name = nameof(DashboardResource.StringDate), ResourceType = typeof(DashboardResource))]
		public Nullable<DateTime> ReleaseDate { get; set; }

		[Display(Name = nameof(DashboardResource.StringIsActive), ResourceType = typeof(DashboardResource))]
		public bool IsActive { get; set; }
	}
}
