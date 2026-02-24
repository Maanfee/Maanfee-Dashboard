using Maanfee.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class GetRoleViewModel
    {
        [Display(Name = "#")]
        public virtual int RowNum { get; set; }

        public virtual string Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringRole), ResourceType = typeof(DashboardResource))]
        public virtual string Name { get; set; }

        [Display(Name = nameof(DashboardResource.StringNormalizedName), ResourceType = typeof(DashboardResource))]
        public virtual string NormalizedName { get; set; }
    }
}
