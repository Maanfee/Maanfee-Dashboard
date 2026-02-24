using Maanfee.Dashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maanfee.Dashboard.Domain.DAL
{
    public class _BaseContext_SQLite : DbContext
    {
        public _BaseContext_SQLite(DbContextOptions<_BaseContext_SQLite> options) : base(options)
        {
        }

        public virtual DbSet<SysRelease> SysReleases { get; set; }

        public virtual DbSet<SysReleaseFeature> SysReleaseFeatures { get; set; }

        public virtual DbSet<SysReleaseType> SysReleaseTypes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region - System Releases -

            modelBuilder.Entity<SysRelease>(e =>
            {
                e.ToTable("SysRelease");
                e.HasIndex(p => new { p.Version }).IsUnique(true);
            });

            #endregion

            #region - System ReleaseNote Types -

            modelBuilder.Entity<SysReleaseType>(e =>
            {
                e.ToTable("SysReleaseType");
                e.HasIndex(p => new { p.Title }).IsUnique(true);
            });

            #endregion

            #region - System Release Note -

            modelBuilder.Entity<SysReleaseFeature>(e =>
            {
                e.ToTable("SysReleaseFeature");
                e.HasIndex(p => new { p.Comment, p.IdSysRelease }).IsUnique(true);
            });

            #endregion

        }

    }
}
 