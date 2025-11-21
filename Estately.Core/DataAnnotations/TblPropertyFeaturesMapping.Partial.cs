namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblPropertyFeaturesMappingMetadata))]
    public partial class TblPropertyFeaturesMapping
    {
        public TblPropertyFeaturesMapping()
        {
        }

        private class TblPropertyFeaturesMappingMetadata
        {
            [StringLength(255)]
            [Unicode(false)]
            public string? Value { get; set; }

            [ForeignKey("FeatureID")]
            [InverseProperty("TblPropertyFeaturesMappings")]
            public virtual TblPropertyFeature Feature { get; set; }

            [ForeignKey("PropertyID")]
            [InverseProperty("TblPropertyFeaturesMappings")]
            public virtual TblProperty Property { get; set; }
        }
    }
}
