namespace Estately.Core.Entities;

[Index("UserID", Name = "IX_TblClientProfiles", IsUnique = true)]
public partial class TblClientProfile
{
    public TblClientProfile()
    {
        CreatedAt ??= DateTime.Now;
    }
    [Key]
    public int ClientProfileID { get; set; }

    public int UserID { get; set; }

    [StringLength(255)]
    public string? FirstName { get; set; }

    [StringLength(255)]
    public string? LastName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(800)]
    public string? ProfilePhoto { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("ClientProfile")]
    public virtual ICollection<TblEmployeeClient>? TblEmployeeClients { get; set; } = new List<TblEmployeeClient>();

    [InverseProperty("ClientProfile")]
    public virtual ICollection<TblFavorite>? TblFavorites { get; set; } = new List<TblFavorite>();

    [ForeignKey("UserID")]
    [InverseProperty(nameof(ApplicationUser.ClientProfile))]
    public virtual ApplicationUser User { get; set; }
}