using Maanfee.Dashboard.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class UserGroup
    {
        [Key]
        public string Id { get; set; }

        // ******************************************************************

        [Display(Name = nameof(DashboardResource.StringGroup), ResourceType = typeof(DashboardResource))]
        public int IdGroup { get; set; }

        [ForeignKey("IdGroup")]
        public virtual Group Group { get; set; }

        // ******************************************************************

        [Display(Name = nameof(DashboardResource.StringUser), ResourceType = typeof(DashboardResource))]
        public string IdApplicationUser { get; set; }

        [ForeignKey("IdApplicationUser")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        // ******************************************************************

    }
}
