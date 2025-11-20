using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class EmployeesViewModel
    {
        public int EmployeeID { get; set; }

        [Required]
        public int JobTitleId { get; set; }
        public int? BranchDepartmentId { get; set; }
        public int? ReportsTo { get; set; }
        [Required]
        public int UserID { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public string Age { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string Nationalid { get; set; } = string.Empty;

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
