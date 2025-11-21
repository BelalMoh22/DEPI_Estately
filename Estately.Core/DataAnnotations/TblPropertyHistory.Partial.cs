namespace Estately.Core.Entities;

[MetadataType(typeof(TblPropertyHistoryMetadata))]
public partial class TblPropertyHistory
{
    public TblPropertyHistory()
    {
        CreatedAt ??= DateTime.Now;
    }

    private class TblPropertyHistoryMetadata
    {
        [StringLength(255)]
        public string? OldValue { get; set; }

        [StringLength(255)]
        public string? NewValue { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
    }
}
