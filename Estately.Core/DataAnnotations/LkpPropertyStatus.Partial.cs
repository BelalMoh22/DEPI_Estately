namespace Estately.Core.Entities
{
    [MetadataType(typeof(LkpPropertyStatusMetadata))]
    public partial class LkpPropertyStatus
    {
        public LkpPropertyStatus()
        {
        }

        private class LkpPropertyStatusMetadata
        {
            [Required]
            [StringLength(255)]
            public string StatusName { get; set; }

            [StringLength(255)]
            public string? Description { get; set; }

            [InverseProperty("Status")]
            public virtual ICollection<TblProperty>? TblProperties { get; set; } = new List<TblProperty>();
        }
    }
}
