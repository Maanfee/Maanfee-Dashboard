using System.ComponentModel.DataAnnotations;

namespace Maanfee.Dashboard.Domain.Entities
{
    public class Permission
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();



    }
}
