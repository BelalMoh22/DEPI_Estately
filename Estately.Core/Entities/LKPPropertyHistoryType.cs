namespace Estately.Core.Entities
{
    public partial class LkpPropertyHistoryType
    {
        [Key]
        public int HistoryTypeID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [InverseProperty("HistoryType")]
        public virtual ICollection<TblPropertyHistory> TblPropertyHistories { get; set; } = new List<TblPropertyHistory>();
    }
}