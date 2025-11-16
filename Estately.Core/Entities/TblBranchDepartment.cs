namespace Estately.Core.Entities
{
    public partial class TblBranchDepartment
    {
        [Key]
        public int BranchDepartmentID { get; set; }

        public int BranchID { get; set; }

        public int DepartmentID { get; set; }

        public bool? IsDeleted { get; set; } = false;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("BranchID")]
        [InverseProperty("TblBranchDepartments")]
        public virtual TblBranch? Branch { get; set; }

        [ForeignKey("DepartmentID")]
        [InverseProperty("TblBranchDepartments")]
        public virtual TblDepartment? Department { get; set; }

        [InverseProperty("BranchDepartment")]
        public virtual ICollection<TblEmployee>? TblEmployees { get; set; } = new List<TblEmployee>();
    }
}