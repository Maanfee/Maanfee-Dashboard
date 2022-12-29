using Maanfee.Dashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maanfee.Dashboard.Domain.DAL
{
    public class _BaseContext_SQLite : DbContext, IBaseDbContext
    {
        public _BaseContext_SQLite(DbContextOptions<_BaseContext_SQLite> options) : base(options)
        {
        }

        public virtual DbSet<SystemRelease> SystemReleases { get; set; }

        public virtual DbSet<SystemReleaseNote> SystemReleaseNotes { get; set; }

        public virtual DbSet<SystemReleaseNoteType> SystemReleaseNoteTypes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region - System Releases -

            modelBuilder.Entity<SystemRelease>(e =>
            {
                e.ToTable("SystemRelease");
                e.HasIndex(p => new { p.Version }).IsUnique(true);
            });

            #endregion

            #region - System ReleaseNote Types -

            modelBuilder.Entity<SystemReleaseNoteType>(e =>
            {
                e.ToTable("SystemReleaseNoteType");
                e.HasIndex(p => new { p.Title }).IsUnique(true);
            });

            #endregion

            #region - System Release Note -

            modelBuilder.Entity<SystemReleaseNote>(e =>
            {
                e.ToTable("SystemReleaseNote");
                e.HasIndex(p => new { p.Comment, p.IdSystemRelease }).IsUnique(true);
            });

            #endregion

        }

    }
}
 