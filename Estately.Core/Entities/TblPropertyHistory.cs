namespace Estately.Core.Entities
{

    [Table("TblPropertyHistory")]
    public partial class TblPropertyHistory
    {
        [Key]
        public int HistoryID { get; set; }

        public int PropertyID { get; set; }

        public int? UserID { get; set; }

        public int HistoryTypeID { get; set; }

        [StringLength(255)]
        public string OldValue { get; set; }

        [StringLength(255)]
        public string NewValue { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("HistoryTypeID")]
        [InverseProperty("TblPropertyHistories")]
        public virtual LkpPropertyHistoryType HistoryType { get; set; }

        [ForeignKey("PropertyID")]
        [InverseProperty("TblPropertyHistories")]
        public virtual TblProperty Property { get; set; }

        [ForeignKey("UserID")]
        [InverseProperty("TblPropertyHistories")]
        public virtual TblUser User { get; set; }
    }
}