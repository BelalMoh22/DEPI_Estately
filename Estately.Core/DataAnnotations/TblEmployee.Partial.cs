namespace Estately.Core.Entities;

[MetadataType(typeof(TblEmployeeMetadata))]
public partial class TblEmployee
{
    public TblEmployee()
    {
        HireDate ??= DateTime.Now;
    }

    private class TblEmployeeMetadata
    {
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string Age { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required]
        [StringLength(14)]
        public string Nationalid { get; set; }

        [StringLength(800)]
        public string? ProfilePhoto { get; set; }
    }
}
