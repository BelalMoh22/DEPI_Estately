namespace Estately.Core.Entities
{
    [MetadataType(typeof(LkpDocumentTypeMetadata))]
    public partial class LkpDocumentType
    {
        public LkpDocumentType()
        {
        }

        private class LkpDocumentTypeMetadata
        {
            [Required]
            [StringLength(255)]
            public string Name { get; set; }

            [StringLength(255)]
            public string? Description { get; set; }

            [InverseProperty("DocumentType")]
            public virtual ICollection<TblPropertyDocument>? TblPropertyDocuments { get; set; } = new List<TblPropertyDocument>();
        }
    }
}
