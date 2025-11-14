
namespace Estately.Core.Entities
{
    public partial class LKPPropertyHistoryType
    {
        [Key]
        public int HistoryTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [InverseProperty("HistoryType")]
        public virtual ICollection<TblPropertyHistory> TblPropertyHistories { get; set; } = new List<TblPropertyHistory>();
    }
}