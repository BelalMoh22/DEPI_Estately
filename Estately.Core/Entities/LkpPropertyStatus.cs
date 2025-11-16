namespace Estately.Core.Entities
{
    [Table("LkpPropertyStatus")]
    public partial class LkpPropertyStatus
    {
        [Key]
        public int StatusID { get; set; }

        [Required]
        [StringLength(255)]
        public string StatusName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [InverseProperty("Status")]
        public virtual ICollection<TblProperty>? TblProperties { get; set; } = new List<TblProperty>();
    }
}