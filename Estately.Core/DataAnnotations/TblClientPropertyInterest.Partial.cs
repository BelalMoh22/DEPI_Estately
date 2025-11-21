namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblClientPropertyInterestMetadata))]
    public partial class TblClientPropertyInterest
    {
        public TblClientPropertyInterest()
        {
            InterestDate ??= DateTime.Now;
        }

        private class TblClientPropertyInterestMetadata
        {
            [Column(TypeName = "datetime")]
            public DateTime? InterestDate { get; set; }

            [StringLength(800)]
            public string? Notes { get; set; }

            [ForeignKey("ClientProfileId")]
            [InverseProperty("TblClientPropertyInterests")]
            public virtual TblClientProfile ClientProfile { get; set; }

            [ForeignKey("PropertyId")]
            [InverseProperty("TblClientPropertyInterests")]
            public virtual TblProperty Property { get; set; }
        }
    }
}
