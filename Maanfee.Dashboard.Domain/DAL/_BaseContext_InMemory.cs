using Microsoft.EntityFrameworkCore;

namespace Maanfee.Dashboard.Domain.DAL
{
    public class _BaseContext_InMemory : DbContext, IBaseDbContext
    {
        public _BaseContext_InMemory(DbContextOptions<_BaseContext_InMemory> options) : base(options)
        {
        }
    }
}
