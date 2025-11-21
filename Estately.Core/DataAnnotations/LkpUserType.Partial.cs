namespace Estately.Core.Entities
{
    [MetadataType(typeof(LkpUserTypeMetadata))]
    public partial class LkpUserType
    {
        public LkpUserType()
        {
        }

        private class LkpUserTypeMetadata
        {
            [Required]
            [StringLength(255)]
            public string UserTypeName { get; set; }

            [StringLength(500)]
            public string? Description { get; set; }

            [InverseProperty("UserType")]
            public virtual ICollection<TblUser>? TblUsers { get; set; } = new List<TblUser>();
        }
    }
}
