using Maanfee.Dashboard.Domain.Entities;
using Maanfee.Dashboard.Domain.Entities.Communications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Maanfee.Dashboard.Domain.DAL
{
    public partial class _BaseContext_SQLServer : IdentityDbContext<ApplicationUser>
    {
        public _BaseContext_SQLServer(DbContextOptions<_BaseContext_SQLServer> options) : base(options)
        {
        }

        #region - Identity -

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<IdentityRole> AspNetRoles { get; set; }
        public virtual DbSet<IdentityRoleClaim<string>> AspNetRoleClaims { get; set; }
        public virtual DbSet<IdentityUserClaim<string>> AspNetUserClaims { get; set; }
        public virtual DbSet<IdentityUserLogin<string>> AspNetUserLogins { get; set; }
        public virtual DbSet<IdentityUserRole<string>> AspNetUserRoles { get; set; }
        public virtual DbSet<IdentityUserToken<string>> AspNetUserTokens { get; set; }

        #endregion

        public virtual DbSet<Gender> Genders { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<UserDepartment> UserDepartments { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<UserGroup> UserGroups { get; set; }

        #region - Communications -

        public DbSet<ChatMessage> ChatMessages { get; set; }

        #endregion

        // =================================================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region - Identity -

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                //b.HasIndex(p => new { p.Phrase }).IsUnique(true);
                b.HasIndex(p => new { p.UserName }).IsUnique(true);
                b.HasIndex(p => new { p.PersonalCode }).IsUnique(true);

                b.ToTable("AspNetUsers");
            });

            modelBuilder.Entity<IdentityRole>(b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedName")
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex")
                    .HasFilter("[NormalizedName] IS NOT NULL");

                b.ToTable("AspNetRoles");
            });

            //modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            //{
            //    b.Property<int>("Id")
            //        .ValueGeneratedOnAdd()
            //        .HasColumnType("int")
            //        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //    b.Property<string>("ClaimType")
            //        .HasColumnType("nvarchar(max)");

            //    b.Property<string>("ClaimValue")
            //        .HasColumnType("nvarchar(max)");

            //    b.Property<string>("RoleId")
            //        .IsRequired()
            //        .HasColumnType("nvarchar(450)");

            //    b.HasKey("Id");

            //    b.HasIndex("RoleId");

            //    b.ToTable("AspNetRoleClaims");
            //});

            modelBuilder.Entity<IdentityRoleClaim<string>>(e =>
            {
                e.Property(c => c.Id).HasColumnType("int").IsRequired().UseIdentityColumn().ValueGeneratedOnAdd();
                e.Property(c => c.ClaimType).HasColumnType("nvarchar(300)");
                e.Property(c => c.ClaimValue).HasColumnType("nvarchar(300)");
                e.Property(c => c.RoleId).IsRequired().HasColumnType("nvarchar(450)");

                e.HasIndex(p => new { p.RoleId, p.ClaimType }).IsUnique(true);

                e.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderKey")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("RoleId")
                    .HasColumnType("nvarchar(450)");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Value")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens");
            });

            // Relations

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.HasOne<IdentityRole>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.HasOne<IdentityRole>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            #endregion

            #region - Gender -

            modelBuilder.Entity<Gender>().ToTable("Gender").HasIndex(p => new { p.Sex }).IsUnique(true);

            #endregion

            #region - Department -

            modelBuilder.Entity<Department>().ToTable("Department").HasIndex(p => new { p.Title, p.IdParent }).IsUnique(true);

            #endregion

            #region - User Department -

            modelBuilder.Entity<UserDepartment>().ToTable("UserDepartment")
                .HasIndex(p => new { p.IdApplicationUser, p.IdDepartment })
                .IsUnique(true);

            #endregion

            #region - Group -

            modelBuilder.Entity<Group>().ToTable("Group").HasIndex(p => new { p.Title }).IsUnique(true);

            #endregion

            #region - User Group -

            modelBuilder.Entity<UserGroup>().ToTable("UserGroup")
                .HasIndex(p => new { p.IdApplicationUser, p.IdGroup })
                .IsUnique(true);

            #endregion

            #region - Communications -

            modelBuilder.Entity<ChatMessage>(e =>
            {
                e.ToTable("ChatMessage");
                e.Property(c => c.Id).HasColumnType("nvarchar(450)").ValueGeneratedOnAdd();
                e.HasOne(d => d.FromUser).WithMany(p => p.ChatMessagesFromUsers).HasForeignKey(d => d.IdFromUser).OnDelete(DeleteBehavior.ClientSetNull);
                e.HasOne(d => d.ToUser).WithMany(p => p.ChatMessagesToUsers).HasForeignKey(d => d.IdToUser).OnDelete(DeleteBehavior.ClientSetNull);
            });

            #endregion

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
        }
    }
}
