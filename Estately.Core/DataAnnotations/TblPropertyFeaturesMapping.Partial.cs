namespace Estately.Core.Entities;

[MetadataType(typeof(TblPropertyFeaturesMappingMetadata))]
public partial class TblPropertyFeaturesMapping
{
    public TblPropertyFeaturesMapping()
    {
        IsDeleted ??= false;
    }

    private class TblPropertyFeaturesMappingMetadata
    {
        [StringLength(255)]
        [Unicode(false)]
        public string? Value { get; set; }
    }
}
