using Microsoft.EntityFrameworkCore;

namespace Maanfee.Logging.Domain.DAL
{
    public class _BaseContext_SQLServer : DbContext
	{
		public _BaseContext_SQLServer(DbContextOptions<_BaseContext_SQLServer> options) : base(options)
		{
		}

		public virtual DbSet<LogInfo> LogInfos { get; set; }

		public virtual DbSet<LoggingLevel> LoggingLevels { get; set; }

		public virtual DbSet<LoggingPlatform> LoggingPlatforms { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			#region - Log Info -

			modelBuilder.Entity<LogInfo>(e =>
			{
				e.ToTable("LogInfo");
				e.Property(c => c.Id).HasColumnType("nvarchar(450)").ValueGeneratedOnAdd();
                e.HasOne(t => t.LoggingLevel).WithMany(t => t.LogInfos).HasForeignKey(d => d.IdLoggingLevel).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(t => t.LoggingPlatform).WithMany(t => t.LogInfos).HasForeignKey(d => d.IdLoggingPlatform).OnDelete(DeleteBehavior.Restrict);
            });

			#endregion

			#region - Logging Level -

			modelBuilder.Entity<LoggingLevel>(e =>
			{
				e.ToTable("LoggingLevel");
				e.Property(c => c.Id).ValueGeneratedOnAdd();
				e.HasIndex(p => new { p.Title }).IsUnique(true);
			});

			#endregion

			#region - Logging Platform -

			modelBuilder.Entity<LoggingPlatform>(e =>
			{
				e.ToTable("LoggingPlatform");
				e.Property(c => c.Id).ValueGeneratedOnAdd();
				e.HasIndex(p => new { p.Title }).IsUnique(true);
			});

			#endregion

		}

	}
}
