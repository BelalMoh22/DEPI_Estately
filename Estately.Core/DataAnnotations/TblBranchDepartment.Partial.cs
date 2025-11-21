namespace Estately.Core.Entities;

[MetadataType(typeof(TblBranchDepartmentMetadata))]
public partial class TblBranchDepartment
{
    public TblBranchDepartment()
    {
        IsDeleted ??= false;
        CreatedAt ??= DateTime.Now;
    }

    private class TblBranchDepartmentMetadata
    {
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
    }
}
