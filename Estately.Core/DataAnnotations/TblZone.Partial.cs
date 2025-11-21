namespace Estately.Core.Entities;

[MetadataType(typeof(TblZoneMetadata))]
public partial class TblZone
{
    public TblZone()
    {
        IsDeleted ??= false;
    }

    private class TblZoneMetadata
    {
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string ZoneName { get; set; }
    }
}
