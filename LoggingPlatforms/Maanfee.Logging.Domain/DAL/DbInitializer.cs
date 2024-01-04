using Microsoft.EntityFrameworkCore;

namespace Maanfee.Logging.Domain.DAL
{
	public static class DbInitializer<TContext> where TContext : DbContext
	{
		public static void DatabaseCreating(TContext context)
		{
			context.Database.EnsureCreated();
		}

	}
}
