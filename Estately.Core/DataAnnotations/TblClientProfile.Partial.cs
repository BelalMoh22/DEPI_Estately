namespace Estately.Core.Entities;

[MetadataType(typeof(TblClientProfileMetadata))]
public partial class TblClientProfile
{
    public TblClientProfile()
    {
        CreatedAt ??= DateTime.Now;
        IsDeleted ??= false;
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
    }
}
