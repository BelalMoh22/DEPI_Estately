namespace Estately.Core.Entities;

[MetadataType(typeof(TblPropertyImageMetadata))]
public partial class TblPropertyImage
{
    public TblPropertyImage()
    {
        UploadedDate ??= DateTime.Now;
    }

    private class TblPropertyImageMetadata
    {
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string ImagePath { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UploadedDate { get; set; }
    }
}
