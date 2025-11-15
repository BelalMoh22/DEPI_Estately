namespace Estately.Core.Entities
{

    [Index("CityID", Name = "IX_TblZones_CityID")]
    public partial class TblZone
    {
        [Key]
        public int ZoneID { get; set; }

        public int CityID { get; set; }

        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string ZoneName { get; set; }

        public bool? IsDeleted { get; set; }

        [ForeignKey("CityID")]
        [InverseProperty("TblZones")]
        public virtual TblCity City { get; set; }

        [InverseProperty("Zone")]
        public virtual ICollection<TblProperty> TblProperties { get; set; } = new List<TblProperty>();
    }
}