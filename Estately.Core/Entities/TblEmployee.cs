namespace Estately.Core.Entities
{

    [Index("UserID", Name = "IX_TblEmployees", IsUnique = true)]
    public partial class TblEmployee
    {
        [Key]
        public int EmployeeID { get; set; }

        public int UserID { get; set; }

        public int? BranchDepartmentId { get; set; }

        public int JobTitleId { get; set; }

        public int? ReportsTo { get; set; }

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
        [StringLength(50)]
        public string Age { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required]
        [StringLength(14)]
        public string Nationalid { get; set; }

        [StringLength(800)]
        public string ProfilePhoto { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Salary { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? HireDate { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

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