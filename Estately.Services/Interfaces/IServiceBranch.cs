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
        Task<BranchesListViewModel> GetBranchPagedAsync(int page, int pageSize, string? search);
        Task<BranchesViewModel?> GetBranchByIdAsync(int? id);
        Task CreateBranchAsync(BranchesViewModel model);
        Task UpdateBranchAsync(BranchesViewModel model);
        Task DeleteBranchAsync(int id);
        Task<int> GetBranchCounterAsync();
        Task<IEnumerable<TblEmployee>> GetAllManagersAsync();
        int GetMaxIDAsync();
        ValueTask<IEnumerable<TblBranch>> SearchZoneAsync(Expression<Func<TblBranch, bool>> predicate);
    }
}
