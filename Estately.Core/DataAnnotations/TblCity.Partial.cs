namespace Estately.Core.Entities;

[MetadataType(typeof(TblCityMetadata))]
public partial class TblCity
{
    public TblCity()
    {
    }

    private class TblCityMetadata
    {
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string CityName { get; set; }
    }
}
