namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblPropertyFeatureMetadata))]
    public partial class TblPropertyFeature
    {
        public TblPropertyFeature()
        {
            CreatedAt ??= DateTime.Now;
        }

        private class TblPropertyFeatureMetadata
        {
            [Required]
            [StringLength(255)]
            [Unicode(false)]
            public string FeatureName { get; set; }

            [StringLength(800)]
            public string? Description { get; set; }

            [Column(TypeName = "datetime")]
            public DateTime? CreatedAt { get; set; }

            [InverseProperty("Feature")]
            public virtual ICollection<TblPropertyFeaturesMapping> TblPropertyFeaturesMappings { get; set; } = new List<TblPropertyFeaturesMapping>();
        }
    }
}