
namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class GetRoleClaimViewModel
    {
        public virtual int Id { get; set; }

        public virtual string RoleId { get; set; }

        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }
    }
}
