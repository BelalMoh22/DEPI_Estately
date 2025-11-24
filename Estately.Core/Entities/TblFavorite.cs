namespace Estately.Core.Entities;

[Index("ClientProfileID", Name = "IX_TblFavorites_ClientProfileID")]
[Index("PropertyID", Name = "IX_TblFavorites_PropertyID")]
public partial class TblFavorite
{
    public TblFavorite()
    {
        CreatedAt ??= DateTime.Now;
    }
    [Key]
    public int FavoriteID { get; set; }

    public int ClientProfileID { get; set; }

    public int PropertyID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ClientProfileID")]
    [InverseProperty("TblFavorites")]
    public virtual TblClientProfile ClientProfile { get; set; }

    [ForeignKey("PropertyID")]
    [InverseProperty("TblFavorites")]
    public virtual TblProperty Property { get; set; }
}