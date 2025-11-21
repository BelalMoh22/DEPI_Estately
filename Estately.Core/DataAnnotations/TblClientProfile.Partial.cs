namespace Estately.Core.Entities;

[MetadataType(typeof(TblClientProfileMetadata))]
public partial class TblClientProfile
{
    public TblClientProfile()
    {
        CreatedAt ??= DateTime.Now;
    }

    private class TblClientProfileMetadata
    {
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
        public virtual ICollection<TblClientPropertyInterest>? TblClientPropertyInterests { get; set; } = new List<TblClientPropertyInterest>();

        [InverseProperty("ClientProfile")]
        public virtual ICollection<TblEmployeeClient>? TblEmployeeClients { get; set; } = new List<TblEmployeeClient>();

        [InverseProperty("ClientProfile")]
        public virtual ICollection<TblFavorite>? TblFavorites { get; set; } = new List<TblFavorite>();

        [ForeignKey("UserID")]
        [InverseProperty("TblClientProfile")]
        public virtual TblUser? User { get; set; }
    }
}
