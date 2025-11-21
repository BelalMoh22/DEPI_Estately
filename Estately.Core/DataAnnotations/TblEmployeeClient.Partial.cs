namespace Estately.Core.Entities;

[MetadataType(typeof(TblEmployeeClientMetadata))]
public partial class TblEmployeeClient
{
    public TblEmployeeClient()
    {
        AssignmentDate ??= DateTime.Now;
    }

    private class TblEmployeeClientMetadata
    {
        [Column(TypeName = "datetime")]
        public DateTime? AssignmentDate { get; set; }
    }
}
