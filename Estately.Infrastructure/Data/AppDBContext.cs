using Microsoft.EntityFrameworkCore.Design;
namespace Estately.Infrastructure.Data
{
    public class AppDBContextFactory : IDesignTimeDbContextFactory<AppDBContext>
    {
        public AppDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();

            var connectionString =
                "Server=DESKTOP-70EG9S0;Initial Catalog=EstatelyDB;Integrated Security=True;Trust Server Certificate=True;";
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDBContext(optionsBuilder.Options);
        }
    }

    public partial class AppDBContext
      : IdentityDbContext<
          ApplicationUser,
          ApplicationRole,
          int,
          ApplicationUserClaim,
          ApplicationUserRole,
          ApplicationUserLogin,
          ApplicationRoleClaim,
          ApplicationUserToken>
    {
        public AppDBContext() { }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options) { }

        #region DbSets
        public virtual DbSet<LkpAppointmentStatus> LkpAppointmentStatuses { get; set; }
        public virtual DbSet<LkpDocumentType> LkpDocumentTypes { get; set; }
        public virtual DbSet<LkpPropertyHistoryType> LkpPropertyHistoryTypes { get; set; }
        public virtual DbSet<LkpPropertyStatus> LkpPropertyStatuses { get; set; }
        public virtual DbSet<LkpPropertyType> LkpPropertyTypes { get; set; }
        public virtual DbSet<LkpUserType> LkpUserTypes { get; set; }

        public virtual DbSet<TblAppointment> TblAppointments { get; set; }
        public virtual DbSet<TblBranch> TblBranches { get; set; }
        public virtual DbSet<TblBranchDepartment> TblBranchDepartments { get; set; }
        public virtual DbSet<TblCity> TblCities { get; set; }
        public virtual DbSet<TblClientProfile> TblClientProfiles { get; set; }
        public virtual DbSet<TblClientPropertyInterest> TblClientPropertyInterests { get; set; }
        public virtual DbSet<TblDepartment> TblDepartments { get; set; }
        public virtual DbSet<TblDeveloperProfile> TblDeveloperProfiles { get; set; }
        public virtual DbSet<TblEmployee> TblEmployees { get; set; }
        public virtual DbSet<TblEmployeeClient> TblEmployeeClients { get; set; }
        public virtual DbSet<TblFavorite> TblFavorites { get; set; }
        public virtual DbSet<TblJobTitle> TblJobTitles { get; set; }
        public virtual DbSet<TblProperty> TblProperties { get; set; }
        public virtual DbSet<TblPropertyDocument> TblPropertyDocuments { get; set; }
        public virtual DbSet<TblPropertyFeature> TblPropertyFeatures { get; set; }
        public virtual DbSet<TblPropertyFeaturesMapping> TblPropertyFeaturesMappings { get; set; }
        public virtual DbSet<TblPropertyHistory> TblPropertyHistories { get; set; }
        public virtual DbSet<TblPropertyImage> TblPropertyImages { get; set; }
        public virtual DbSet<TblZone> TblZones { get; set; }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===============================
            // IDENTITY TABLES
            // ===============================
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("AspNetUserClaims");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<ApplicationRoleClaim>().ToTable("AspNetRoleClaims");
            modelBuilder.Entity<ApplicationUserToken>().ToTable("AspNetUserTokens");
            modelBuilder.Entity<TblClientProfile>()
                .HasOne(p => p.User)
                .WithOne(u => u.ClientProfile)
                .HasForeignKey<TblClientProfile>(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TblDeveloperProfile>()
                .HasOne(p => p.User)
                .WithOne(u => u.DeveloperProfile)
                .HasForeignKey<TblDeveloperProfile>(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TblEmployee>()
                .HasOne(e => e.User)
                .WithOne(u => u.EmployeeProfile)
                .HasForeignKey<TblEmployee>(e => e.UserID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.UserType)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.UserTypeID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TblPropertyDocument>()
                .HasOne(d => d.User)
                .WithMany(u => u.PropertyDocuments)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TblPropertyHistory>()
                .HasOne(h => h.User)
                .WithMany(u => u.PropertyHistories)
                .HasForeignKey(h => h.UserID)
                .OnDelete(DeleteBehavior.Restrict);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
