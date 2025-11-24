using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.ViewModels
{
    public class DepartmentsViewModel
    {
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Department Name is required")]
        [StringLength(200, ErrorMessage = "Department Name cannot exceed 200 characters")]
        public string DepartmentName { get; set; } = string.Empty;
        [Display(Name = "Manager Name")]
        public string? ManagerName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string? Email { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual TblJobTitle? JobTitle { get; set; }
    }
    public class DepartmentsListViewModel : BaseViewModel
    {
        public List<DepartmentsViewModel> Departments { get; set; } = new();
    }
}
