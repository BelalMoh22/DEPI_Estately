namespace Estately.Core.Entities;

[Index("UserID", Name = "IX_TblDeveloperProfiles", IsUnique = true)]
public partial class TblDeveloperProfile
{
    public TblDeveloperProfile()
    {
        CreatedAt ??= DateTime.Now;
    }
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

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("DeveloperProfile")]
    public virtual ICollection<TblProperty> TblProperties { get; set; } = new List<TblProperty>();

    [ForeignKey("UserID")]
    [InverseProperty(nameof(ApplicationUser.DeveloperProfile))]
    public virtual ApplicationUser User { get; set; }

}