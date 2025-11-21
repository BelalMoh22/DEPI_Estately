namespace Estately.Core.Entities
{
    [MetadataType(typeof(TblFavoriteMetadata))]
    public partial class TblFavorite
    {
        public TblFavorite()
        {
            CreatedAt ??= DateTime.Now;
        }
        private class TblFavoriteMetadata
        {
            [Column(TypeName = "datetime")]
            public DateTime? CreatedAt { get; set; }

            [ForeignKey("ClientProfileID")]
            [InverseProperty("TblFavorites")]
            public virtual TblClientProfile ClientProfile { get; set; }

            [ForeignKey("PropertyID")]
            [InverseProperty("TblFavorites")]
            public virtual TblProperty Property { get; set; }
        }
    }
}
