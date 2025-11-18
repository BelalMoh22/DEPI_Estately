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
        public string DepartmentName { get; set; } = string.Empty;
        public string? ManagerName { get; set; }
        public string? Email { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual TblJobTitle? JobTitle { get; set; }
    }
    public class DepartmentsListViewModel : BaseViewModel
    {
        public List<DepartmentsViewModel> Departments { get; set; } = new();
    }
}
