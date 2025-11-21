namespace Estately.Core.Entities;

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
    }
}
