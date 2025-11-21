namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblZoneMetadata))]
    public partial class TblZone
    {
        public TblZone()
        {
        }

        private class TblZoneMetadata
        {
            [Required]
            [StringLength(255)]
            [Unicode(false)]
            public string ZoneName { get; set; }

            [ForeignKey("CityID")]
            [InverseProperty("TblZones")]
            public virtual TblCity? City { get; set; }

            [InverseProperty("Zone")]
            public virtual ICollection<TblProperty> TblProperties { get; set; } = new List<TblProperty>();
        }
    }
}
