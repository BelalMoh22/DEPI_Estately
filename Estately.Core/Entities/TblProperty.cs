namespace Estately.Core.Entities;

[Index("PropertyCode", Name = "IX_TblProperties_1", IsUnique = true)]
[Index("DeveloperProfileID", Name = "IX_TblProperties_DeveloperProfileID")]
[Index("PropertyTypeID", Name = "IX_TblProperties_PropertyTypeID")]
[Index("ZoneID", Name = "IX_TblProperties_ZoneID")]
public partial class TblProperty
{
    public TblProperty()
    {
        StatusId ??= 1; // Default status
        ExpectedRentPrice ??= 0m;
        IsDeleted ??= false;
        ListingDate = DateTime.Now;
    }
    [Key]
    public int PropertyID { get; set; }

    public int? AgentId { get; set; }

    public int? DeveloperProfileID { get; set; }

    public int PropertyTypeID { get; set; }

    public int? StatusId { get; set; }

    public int ZoneID { get; set; }

    public int? YearBuilt { get; set; } = null;

    [Required]
    [StringLength(255)]
    public string Address { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }


    [Range(0, int.MaxValue)]
    public int? FloorNo { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int BedsNo { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int BathsNo { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Area { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ListingDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? ExpectedRentPrice { get; set; }
    public bool? IsDeleted { get; set; }

    [Required]
    [Column(TypeName = "decimal(9, 6)")]
    public decimal Latitude { get; set; }

    [Required]
    [Column(TypeName = "decimal(9, 6)")]
    public decimal Longitude { get; set; }

    [Required]
    [StringLength(50)]
    public string PropertyCode { get; set; }


    [ForeignKey("AgentId")]
    [InverseProperty("TblProperties")]
    public virtual TblEmployee Agent { get; set; }

    [ForeignKey("DeveloperProfileID")]
    [InverseProperty("TblProperties")]
    public virtual TblDeveloperProfile DeveloperProfile { get; set; }

    [ForeignKey("PropertyTypeID")]
    [InverseProperty("TblProperties")]
    public virtual LkpPropertyType PropertyType { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("TblProperties")]
    public virtual LkpPropertyStatus? Status { get; set; }

    [InverseProperty("Property")]
    public virtual ICollection<TblAppointment> TblAppointments { get; set; } = new List<TblAppointment>();

    [InverseProperty("Property")]
    public virtual ICollection<TblFavorite> TblFavorites { get; set; } = new List<TblFavorite>();

    [InverseProperty("Property")]
    public virtual ICollection<TblPropertyFeaturesMapping> TblPropertyFeaturesMappings { get; set; } = new List<TblPropertyFeaturesMapping>();

    [InverseProperty("Property")]
    public virtual ICollection<TblPropertyImage> TblPropertyImages { get; set; } = new List<TblPropertyImage>();

    [ForeignKey("ZoneID")]
    [InverseProperty("TblProperties")]
    public virtual TblZone Zone { get; set; }
}