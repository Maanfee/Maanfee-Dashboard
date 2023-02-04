
namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class SubmitRoleClaimViewModel
    {
        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }

        public virtual int Id { get; set; }

        public virtual string RoleId { get; set; }

        public virtual bool IsSelected { get; set; }

        public virtual string Action { get; set; }

    }
}
