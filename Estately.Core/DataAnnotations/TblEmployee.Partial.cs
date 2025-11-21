namespace Estately.Core.Entities
{

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
            [Range(18, 65)]
            public int Age { get; set; }

            [Required]
            [StringLength(50)]
            public string Phone { get; set; }

            [Required]
            [StringLength(14)]
            [RegularExpression("^[0-9]*$", ErrorMessage = "National ID must be numeric.")]
            public string Nationalid { get; set; }

            [NotMapped]
            public byte[] ProfilePhotoBytes { get; set; }

            [StringLength(800)]
            public string? ProfilePhoto { get; set; }

            [Column(TypeName = "datetime")]
            public DateTime? HireDate { get; set; }

            [Column(TypeName = "decimal(18, 2)")]
            public decimal Salary { get; set; }
            [ForeignKey("BranchDepartmentId")]
            [InverseProperty("TblEmployees")]
            public virtual TblBranchDepartment BranchDepartment { get; set; }

            [InverseProperty("ReportsToNavigation")]
            public virtual ICollection<TblEmployee> InverseReportsToNavigation { get; set; } = new List<TblEmployee>();

            [ForeignKey("JobTitleId")]
            [InverseProperty("TblEmployees")]
            public virtual TblJobTitle JobTitle { get; set; }

            [ForeignKey("ReportsTo")]
            [InverseProperty("InverseReportsToNavigation")]
            public virtual TblEmployee ReportsToNavigation { get; set; }

            [InverseProperty("Employee")]
            public virtual ICollection<TblEmployeeClient> TblEmployeeClients { get; set; } = new List<TblEmployeeClient>();

            [InverseProperty("Agent")]
            public virtual ICollection<TblProperty> TblProperties { get; set; } = new List<TblProperty>();

            [ForeignKey("UserID")]
            [InverseProperty("TblEmployee")]
            public virtual TblUser User { get; set; }
        }
    }
}