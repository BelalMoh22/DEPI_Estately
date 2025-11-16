namespace Estately.Core.Entities
{
    public partial class TblJobTitle
    {
        [Key]
        public int JobTitleId { get; set; }

        [Required]
        [StringLength(255)]
        public string JobTitleName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public bool? IsDeleted { get; set; } = false;

        [InverseProperty("JobTitle")]
        public virtual ICollection<TblEmployee>? TblEmployees { get; set; } = new List<TblEmployee>();
    }
}