using Maanfee.Dashboard.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class SystemReleaseNote
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = nameof(DashboardResource.StringComment), ResourceType = typeof(DashboardResource))]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceName = nameof(DashboardResource.ValidationStringLength), ErrorMessageResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string Comment { get; set; }

        // ===============================================

        [Display(Name = nameof(DashboardResource.StringRelease), ResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public string IdSystemRelease { get; set; }

        [ForeignKey("IdSystemRelease")]
        public virtual SystemRelease SystemRelease { get; set; }

        // ===============================================

        [Display(Name = nameof(DashboardResource.StringReleaseNoteType), ResourceType = typeof(DashboardResource))]
        [Required(ErrorMessageResourceName = nameof(DashboardResource.ValidationRequired), ErrorMessageResourceType = typeof(DashboardResource))]
        public int IdSystemReleaseNoteType { get; set; }

        [ForeignKey("IdSystemReleaseNoteType")]
        public virtual SystemReleaseNoteType SystemReleaseNoteType { get; set; }

        // ===============================================
    }
}
