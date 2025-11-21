namespace Estately.Core.Entities;

[MetadataType(typeof(TblBranchDepartmentMetadata))]
public partial class TblBranchDepartment
{
    public TblBranchDepartment()
    {
        CreatedAt ??= DateTime.Now;
    }

    private class TblBranchDepartmentMetadata
    {
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
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
