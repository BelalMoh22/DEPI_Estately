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
        public int? UserTypeID { get; set; }

        [ForeignKey(nameof(UserTypeID))]
        public virtual LkpUserType? UserType { get; set; }
    }
}
