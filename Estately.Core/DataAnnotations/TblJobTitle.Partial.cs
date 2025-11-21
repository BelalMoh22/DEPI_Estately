namespace Estately.Core.Entities;

[MetadataType(typeof(TblJobTitleMetadata))]
public partial class TblJobTitle
{
    public TblJobTitle()
    {
        IsDeleted ??= false;
    }

    private class TblJobTitleMetadata
    {
        [Required]
        [StringLength(255)]
        public string JobTitleName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
