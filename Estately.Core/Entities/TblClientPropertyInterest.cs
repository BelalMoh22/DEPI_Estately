namespace Estately.Core.Entities;
public partial class TblClientPropertyInterest
{
    public TblClientPropertyInterest()
    {
        InterestDate ??= DateTime.Now;
    }

    [Key]
    public int InterestId { get; set; }

    public int ClientProfileId { get; set; }

    public int PropertyId { get; set; }

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