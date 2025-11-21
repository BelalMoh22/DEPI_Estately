namespace Estately.Core.Entities
{
    [MetadataType(typeof(LkpPropertyHistoryTypeMetadata))]
    public partial class LkpPropertyHistoryType
    {
        public LkpPropertyHistoryType() { }

        private class LkpPropertyHistoryTypeMetadata
        {
            [Required]
            [StringLength(255)]
            public string Name { get; set; }

            [StringLength(255)]
            public string? Description { get; set; }

            [InverseProperty("HistoryType")]
            public virtual ICollection<TblPropertyHistory>? TblPropertyHistories { get; set; } = new List<TblPropertyHistory>();
        }
    }
}