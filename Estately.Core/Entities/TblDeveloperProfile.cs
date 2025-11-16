namespace Estately.Core.Entities
{
    [Index("UserID", Name = "IX_TblDeveloperProfiles", IsUnique = true)]
    public partial class TblDeveloperProfile
    {
        [Key]
        public int DeveloperProfileID { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string DeveloperTitle { get; set; }

        [StringLength(255)]
        public string? DeveloperName { get; set; }

        [StringLength(800)]
        [Unicode(false)]
        public string? WebsiteURL { get; set; }

        [StringLength(800)]
        [Unicode(false)]
        public string? PortofolioPhoto { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        public bool? IsDeleted { get; set; } = false;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [InverseProperty("DeveloperProfile")]
        public virtual ICollection<TblProperty>? TblProperties { get; set; } = new List<TblProperty>();

        [ForeignKey("UserID")]
        [InverseProperty("TblDeveloperProfile")]
        public virtual TblUser? User { get; set; }
    }
}