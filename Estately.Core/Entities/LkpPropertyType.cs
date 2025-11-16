namespace Estately.Core.Entities
{
    public partial class LkpPropertyType
    {
        [Key]
        public int PropertyTypeID { get; set; }

        [Required]
        [StringLength(255)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [InverseProperty("PropertyType")]
        public virtual ICollection<TblProperty>? TblProperties { get; set; } = new List<TblProperty>();
    }
}