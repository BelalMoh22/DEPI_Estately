using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.Interfaces
{
    public interface IServiceBranch
    {
        Task<BranchListViewModel> GetBranchPagedAsync(int page, int pageSize, string? search);
        Task<BranchViewModel?> GetBranchByIdAsync(int? id);
        Task CreateBranchAsync(BranchViewModel model);
        Task UpdateBranchAsync(BranchViewModel model);
        Task DeleteBranchAsync(int id);
        Task<int> GetBranchCounterAsync();
        Task<IEnumerable<TblEmployee>> GetAllManagersAsync();
        Task<bool> BranchHasEmployeesAsync(int branchId);
        Task<bool> BranchNameExistsAsync(string branchName, int? branchId);
        Task<bool> ManagerAssignedElsewhereAsync(string managerName, int? branchId);
        Task<bool> BranchPhoneExistsAsync(string phone, int? branchId);
        int GetMaxIDAsync();
        ValueTask<IEnumerable<TblBranch>> SearchZoneAsync(Expression<Func<TblBranch, bool>> predicate);
    }
}
