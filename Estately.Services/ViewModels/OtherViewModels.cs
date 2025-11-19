using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{

    public class EmployeeViewModel
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "User is required")]
        [Display(Name = "User")]
        public int UserID { get; set; }

        [Display(Name = "Branch Department")]
        public int? BranchDepartmentId { get; set; }

        [Required(ErrorMessage = "Job Title is required")]
        [Display(Name = "Job Title")]
        public int JobTitleId { get; set; }

        [Display(Name = "Reports To")]
        public int? ReportsTo { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(255)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(50)]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required")]
        [StringLength(50)]
        public string Age { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "National ID is required")]
        [StringLength(14)]
        [Display(Name = "National ID")]
        public string Nationalid { get; set; } = string.Empty;

        [StringLength(800)]
        [Display(Name = "Profile Photo")]
        public string? ProfilePhoto { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(0, double.MaxValue)]
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }

        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "Branch Department Name")]
        public string? BranchDepartmentName { get; set; }

        [Display(Name = "Job Title Name")]
        public string? JobTitleName { get; set; }

        [Display(Name = "Reports To Name")]
        public string? ReportsToName { get; set; }

        [Display(Name = "Username")]
        public string? Username { get; set; }
    }

    public class ClientProfileViewModel
    {
        public int ClientProfileID { get; set; }

        [Required(ErrorMessage = "User is required")]
        [Display(Name = "User")]
        public int UserID { get; set; }

        [StringLength(255)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(255)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(800)]
        [Display(Name = "Profile Photo")]
        public string? ProfilePhoto { get; set; }

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Is Deleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "Username")]
        public string? Username { get; set; }
    }

    public class DeveloperProfileViewModel
    {
        public int DeveloperProfileID { get; set; }

        [Required(ErrorMessage = "User is required")]
        [Display(Name = "User")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Developer Name is required")]
        [StringLength(255)]
        [Display(Name = "Developer Name")]
        public string DeveloperName { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Website URL")]
        [Url(ErrorMessage = "Invalid URL")]
        public string? WebsiteURL { get; set; }

        [StringLength(800)]
        [Display(Name = "Portfolio Photo")]
        public string? PortofolioPhoto { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [Display(Name = "Username")]
        public string? Username { get; set; }
    }

    public class AppointmentsViewModel
    {
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public int StatusID { get; set; }

        [Required(ErrorMessage = "Property is required")]
        [Display(Name = "Property")]
        public int PropertyID { get; set; }

        [Required(ErrorMessage = "Employee Client is required")]
        [Display(Name = "Employee Client")]
        public int EmployeeClientID { get; set; }

        [Required(ErrorMessage = "Appointment Date is required")]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        public string? Notes { get; set; }

        [Display(Name = "Status Name")]
        public string? StatusName { get; set; }

        [Display(Name = "Property Title")]
        public string? PropertyTitle { get; set; }

        [Display(Name = "Client Name")]
        public string? ClientName { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }
    }
    public class PropertyImageViewModel
    {
        public int ImageID { get; set; }

        [Required(ErrorMessage = "Property is required")]
        [Display(Name = "Property")]
        public int PropertyID { get; set; }

        [Required(ErrorMessage = "Image Path is required")]
        [StringLength(255)]
        [Display(Name = "Image Path")]
        public string ImagePath { get; set; } = string.Empty;

        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; } = DateTime.Now;

        [Display(Name = "Property Title")]
        public string? PropertyTitle { get; set; }
    }

    public class PropertyDocumentViewModel
    {
        public int DocumentID { get; set; }

        [Required(ErrorMessage = "Property is required")]
        [Display(Name = "Property")]
        public int PropertyID { get; set; }

        [Display(Name = "User")]
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Document Type is required")]
        [Display(Name = "Document Type")]
        public int DocumentTypeID { get; set; }

        [Required(ErrorMessage = "File Path is required")]
        [StringLength(300)]
        [Display(Name = "File Path")]
        public string FilePath { get; set; } = string.Empty;

        [Display(Name = "Uploaded At")]
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Notes { get; set; }

        [Display(Name = "Property Title")]
        public string? PropertyTitle { get; set; }

        [Display(Name = "Document Type")]
        public string? DocumentTypeName { get; set; }

        [Display(Name = "Username")]
        public string? Username { get; set; }
    }

    public class PropertyFeatureViewModel
    {
        public int FeatureID { get; set; }

        [Required(ErrorMessage = "Feature Name is required")]
        [StringLength(255)]
        [Display(Name = "Feature Name")]
        public string FeatureName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

    public class FavoriteViewModel
    {
        public int FavoriteID { get; set; }

        [Required(ErrorMessage = "Client Profile is required")]
        [Display(Name = "Client Profile")]
        public int ClientProfileID { get; set; }

        [Required(ErrorMessage = "Property is required")]
        [Display(Name = "Property")]
        public int PropertyID { get; set; }

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Is Deleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "Client Name")]
        public string? ClientName { get; set; }

        [Display(Name = "Property Title")]
        public string? PropertyTitle { get; set; }
    }

    public class EmployeeClientViewModel
    {
        public int EmployeeClientID { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        [Display(Name = "Employee")]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Client Profile is required")]
        [Display(Name = "Client Profile")]
        public int ClientProfileID { get; set; }

        [Display(Name = "Assignment Date")]
        public DateTime? AssignmentDate { get; set; } = DateTime.Now;

        [Display(Name = "Is Deleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }

        [Display(Name = "Client Name")]
        public string? ClientName { get; set; }
    }

    public class BranchDepartmentViewModel
    {
        public int BranchDepartmentID { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [Display(Name = "Branch Name")]
        public string? BranchName { get; set; }

        [Display(Name = "Department Name")]
        public string? DepartmentName { get; set; }
    }

    public class PropertyHistoryViewModel
    {
        public int HistoryID { get; set; }

        [Required(ErrorMessage = "Property is required")]
        [Display(Name = "Property")]
        public int PropertyID { get; set; }

        [Display(Name = "User")]
        public int? UserID { get; set; }

        [Required(ErrorMessage = "History Type is required")]
        [Display(Name = "History Type")]
        public int HistoryTypeID { get; set; }

        [Display(Name = "Old Value")]
        public string? OldValue { get; set; }

        [Display(Name = "New Value")]
        public string? NewValue { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Property Title")]
        public string? PropertyTitle { get; set; }

        [Display(Name = "History Type")]
        public string? HistoryTypeName { get; set; }

        [Display(Name = "Username")]
        public string? Username { get; set; }
    }
}

