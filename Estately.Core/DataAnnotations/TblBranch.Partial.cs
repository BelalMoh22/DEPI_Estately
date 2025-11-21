namespace Estately.Core.Entities;

[MetadataType(typeof(TblBranchMetadata))]
public partial class TblBranch
{
    public TblBranch()
    {
    }

    private class TblBranchMetadata
    {
        [Required]
        [StringLength(255)]
        public string BranchName { get; set; }

        [StringLength(255)]
        public string? ManagerName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Phone { get; set; }

        [InverseProperty("Branch")]
        public virtual ICollection<TblBranchDepartment>? TblBranchDepartments { get; set; } = new List<TblBranchDepartment>();
    }
}
