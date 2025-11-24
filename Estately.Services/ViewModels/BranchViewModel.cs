using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.ViewModels
{
    public class BranchViewModel
    {
        public int BranchID { get; set; }
        [Required(ErrorMessage = "Branch name is required.")]
        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters.")]
        public string BranchName { get; set; } = string.Empty;
        [StringLength(100, ErrorMessage = "Manager name cannot exceed 100 characters.")]
        public string? ManagerName { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20, ErrorMessage = "Phone number is too long.")]
        public string Phone { get; set; } = string.Empty;
        public bool? IsDeleted { get; set; } = false;
        public virtual TblJobTitle? JobTitle { get; set; }
    }

    public class BranchListViewModel : BaseViewModel
    {
        public List<BranchViewModel> Branches { get; set; } = new();
    }
}
