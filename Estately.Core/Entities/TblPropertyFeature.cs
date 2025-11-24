namespace Estately.Core.Entities;

public partial class TblPropertyFeature
{
    public TblPropertyFeature()
    {
        CreatedAt ??= DateTime.Now;
    }
    [Key]
    public int FeatureID { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string FeatureName { get; set; }

    [StringLength(800)]
    public string? Description { get; set; }

    public bool? IsAmenity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Feature")]
    public virtual ICollection<TblPropertyFeaturesMapping> TblPropertyFeaturesMappings { get; set; } = new List<TblPropertyFeaturesMapping>();
}