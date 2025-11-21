namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblUserMetadata))]
    public partial class TblUser
    {
        public TblUser()
        {
            UserTypeID ??= 1;
            CreatedAt ??= DateTime.Now;
        }

        private class TblUserMetadata
        {
            [Required]
            [StringLength(255)]
            [Unicode(false)]
            public string Email { get; set; }

            [Required]
            [StringLength(255)]
            public string Username { get; set; }

            [Required]
            [StringLength(500)]
            [Unicode(false)]
            public string PasswordHash { get; set; }

            [Column(TypeName = "datetime")]
            public DateTime? CreatedAt { get; set; }

            [InverseProperty("User")]
            public virtual TblClientProfile? TblClientProfile { get; set; }

            [InverseProperty("User")]
            public virtual TblDeveloperProfile TblDeveloperProfile { get; set; }

            [InverseProperty("User")]
            public virtual TblEmployee? TblEmployee { get; set; }

            [InverseProperty("User")]
            public virtual ICollection<TblPropertyDocument>? TblPropertyDocuments { get; set; } = new List<TblPropertyDocument>();

            [InverseProperty("User")]
            public virtual ICollection<TblPropertyHistory>? TblPropertyHistories { get; set; } = new List<TblPropertyHistory>();

            [ForeignKey("UserTypeID")]
            [InverseProperty("TblUsers")]
            public virtual LkpUserType UserType { get; set; }
        }
    }
}