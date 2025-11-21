namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblJobTitleMetadata))]
    public partial class TblJobTitle
    {
        public TblJobTitle()
        {
        }

        private class TblJobTitleMetadata
        {
            [Required]
            [StringLength(255)]
            public string JobTitleName { get; set; }

            [StringLength(255)]
            public string? Description { get; set; }

            [InverseProperty("JobTitle")]
            public virtual ICollection<TblEmployee> TblEmployees { get; set; } = new List<TblEmployee>();
        }
    }
}