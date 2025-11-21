namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblDepartmentMetadata))]
    public partial class TblDepartment
    {
        public TblDepartment()
        {
            CreatedAt ??= DateTime.Now;
        }

        private class TblDepartmentMetadata
        {
            public string DepartmentName { get; set; }

            [StringLength(100)]
            public string? ManagerName { get; set; }

            [StringLength(255)]
            [Unicode(false)]
            public string? Email { get; set; }

            [StringLength(50)]
            [Unicode(false)]
            public string? Phone { get; set; }

            [Column(TypeName = "datetime")]
            public DateTime? CreatedAt { get; set; }

            [InverseProperty("Department")]
            public virtual ICollection<TblBranchDepartment> TblBranchDepartments { get; set; } = new List<TblBranchDepartment>();
        }
    }
}
