using Maanfee.Dashboard.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class Group
    {
        public Group()
        {
            this.UserGroups = new List<UserGroup>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringGroup), ResourceType = typeof(DashboardResource))]
        [StringLength(150, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Title { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
