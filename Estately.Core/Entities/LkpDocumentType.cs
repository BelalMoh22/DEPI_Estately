namespace Estately.Core.Entities
{
    public partial class LkpDocumentType
    {
        [Key]
        public int DocumentTypeID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [InverseProperty("DocumentType")]
        public virtual ICollection<TblPropertyDocument>? TblPropertyDocuments { get; set; } = new List<TblPropertyDocument>();
    }
}