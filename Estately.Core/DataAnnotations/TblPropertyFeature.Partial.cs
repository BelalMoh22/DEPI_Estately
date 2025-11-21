namespace Estately.Core.Entities;

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
    }
}
