namespace Estately.Core.Entities;

[MetadataType(typeof(TblPropertyMetadata))]
public partial class TblProperty
{
    public TblProperty()
    {
        ListingDate ??= DateTime.Now;
        ExpectedRentPrice ??= 0m;
        IsDeleted ??= false;
        IsFurnished ??= false;
    }

    private class TblPropertyMetadata
    {
        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string PropertyCode { get; set; }
    }
}
