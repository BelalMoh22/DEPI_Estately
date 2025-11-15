namespace Estately.Core.Entities
{
    public partial class TblCity
    {
        [Key]
        public int CityID { get; set; }

        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string CityName { get; set; }

        [InverseProperty("City")]
        public virtual ICollection<TblZone> TblZones { get; set; } = new List<TblZone>();
    }
}