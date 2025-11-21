namespace Estately.Core.Entities;

[MetadataType(typeof(LkpPropertyTypeMetadata))]
public partial class LkpPropertyType
{
    public LkpPropertyType()
    {
    }

    private class LkpPropertyTypeMetadata
    {
        [Required]
        [StringLength(255)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [InverseProperty("PropertyType")]
        public virtual ICollection<TblProperty>? TblProperties { get; set; } = new List<TblProperty>();
    }
}
