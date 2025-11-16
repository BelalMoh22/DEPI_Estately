namespace Estately.Core.Entities
{
    public partial class TblDepartment
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string DepartmentName { get; set; }

        [StringLength(100)]
        public string? ManagerName { get; set; }

        [StringLength(255)]
        [Unicode(false)]
        public string? Email { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? Phone { get; set; }

        public bool? IsDeleted { get; set; } = false;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [InverseProperty("Department")]
        public virtual ICollection<TblBranchDepartment>? TblBranchDepartments { get; set; } = new List<TblBranchDepartment>();
    }
}