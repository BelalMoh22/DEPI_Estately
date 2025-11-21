namespace Estately.Core.Entities;

[MetadataType(typeof(TblPropertyDocumentMetadata))]
public partial class TblPropertyDocument
{
    public TblPropertyDocument()
    {
        UploadedAt ??= DateTime.Now;
    }

    private class TblPropertyDocumentMetadata
    {
        [Required]
        [StringLength(300)]
        public string FilePath { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UploadedAt { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
