namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblDeveloperProfileMetadata))]
    public partial class TblDeveloperProfile
    {
        public TblDeveloperProfile()
        {
            CreatedAt ??= DateTime.Now;
        }

        private class TblDeveloperProfileMetadata
        {
            [Required]
            [StringLength(255)]
            public string DeveloperTitle { get; set; }

   
            public string? DeveloperName { get; set; }

            [StringLength(800)]
            [Unicode(false)]
            public string? WebsiteURL { get; set; }

            [StringLength(800)]
            [Unicode(false)]
            public string? PortofolioPhoto { get; set; }

            [StringLength(50)]
            public string? Phone { get; set; }

            [Column(TypeName = "datetime")]
            public DateTime? CreatedAt { get; set; }

            [InverseProperty("DeveloperProfile")]
            public virtual ICollection<TblProperty> TblProperties { get; set; } = new List<TblProperty>();

            [ForeignKey("UserID")]
            [InverseProperty("TblDeveloperProfile")]
            public virtual TblUser User { get; set; }
        }
    }
}