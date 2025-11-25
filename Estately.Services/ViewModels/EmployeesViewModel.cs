using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class EmployeesViewModel
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Job Title is required")]
        [Display(Name = "Job Title")]
        public int JobTitleId { get; set; }
        public int? BranchDepartmentId { get; set; }
        public int? ReportsTo { get; set; }
        [Required(ErrorMessage = "User is required")]
        [Display(Name = "User")]
        public int UserID { get; set; }

        public string FullName => $"{FirstName} {LastName}";

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
        [Range(18, 65, ErrorMessage = "Age must be between 18 and 65.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "National ID is required")]
        [StringLength(14)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "National ID must be numeric.")]
        public string Nationalid { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get; set; }

        public DateTime? HireDate { get; set; } = DateTime.Now;

        public string? ProfilePhoto { get; set; }
        public IFormFile? UploadedPhoto { get; set; }

        public string JobTitleName { get; set; } = "";
        public string BranchName { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public string? Email { get; set; }

        public IEnumerable<JobTitleViewModel> JobTitles { get; set; } = new List<JobTitleViewModel>();
        public IEnumerable<BranchDepartmentViewModel> BranchDepartments { get; set; } = new List<BranchDepartmentViewModel>();
        public IEnumerable<EmployeeSelectViewModel> Managers { get; set; } = new List<EmployeeSelectViewModel>();
        public IEnumerable<BranchViewModel> Branches { get; set; } = new List<BranchViewModel>();
        public IEnumerable<DepartmentsViewModel> Departments { get; set; } = new List<DepartmentsViewModel>();
        public IEnumerable<UserSelectViewModel> Users { get; set; } = new List<UserSelectViewModel>();
    }

    public class EmployeesListViewModel : BaseViewModel
    {
        public List<EmployeesViewModel> Employees { get; set; } = new();
    }

    public class JobTitleViewModel
    {
        public int JobTitleId { get; set; }
        public string JobTitleName { get; set; } = "";
    }

    public class BranchDepartmentViewModel
    {
        public int BranchDepartmentId { get; set; }
        public string BranchName { get; set; } = "";
        public string DepartmentName { get; set; } = "";
    }

    public class EmployeeSelectViewModel
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = "";
    }

    public class UserSelectViewModel
    {
        public int UserID { get; set; }
        public string Email { get; set; } = "";
    }
}