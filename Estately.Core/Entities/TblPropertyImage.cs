namespace Estately.Core.Entities;

[Index("PropertyID", Name = "IX_TblPropertyImages_PropertyID")]
public partial class TblPropertyImage
{
    public TblPropertyImage()
    {
        UploadedDate ??= DateTime.Now;
    }
    [Key]
    public int ImageID { get; set; }

    public int PropertyID { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string ImagePath { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UploadedDate { get; set; }

    [ForeignKey("PropertyID")]
    [InverseProperty("TblPropertyImages")]
    public virtual TblProperty Property { get; set; }
}