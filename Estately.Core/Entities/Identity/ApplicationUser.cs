namespace Estately.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            UserTypeID ??= 1;
            CreatedAt ??= DateTime.Now;
        }

        public DateTime? CreatedAt { get; set; }

        // --------------------
        // PROFILES (ONE-TO-ONE)
        // --------------------
        public virtual TblClientProfile? ClientProfile { get; set; }
        public virtual TblDeveloperProfile? DeveloperProfile { get; set; }
        public virtual TblEmployee? EmployeeProfile { get; set; }

        // --------------------
        // PROPERTY RELATIONS (ONE-TO-MANY)
        // --------------------
        public virtual ICollection<TblPropertyDocument>? PropertyDocuments { get; set; }
            = new List<TblPropertyDocument>();

        public virtual ICollection<TblPropertyHistory>? PropertyHistories { get; set; }
            = new List<TblPropertyHistory>();

        // --------------------
        // USER TYPE (ONE-TO-MANY)
        // --------------------
        public int? UserTypeID { get; set; }

        [ForeignKey(nameof(UserTypeID))]
        public virtual LkpUserType? UserType { get; set; }
    }
}
