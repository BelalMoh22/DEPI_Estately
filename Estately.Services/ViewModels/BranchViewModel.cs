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
        public string BranchName { get; set; } = string.Empty;
        public string? ManagerName { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool? IsDeleted { get; set; } = false;
        public virtual TblJobTitle? JobTitle { get; set; }
    }

    public class BranchListViewModel : BaseViewModel
    {
        public List<BranchViewModel> Branches { get; set; } = new();
    }
}
