using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    public interface IServicePropertyStatus
    {
        Task<PropertyStatusListViewModel> GetPropertyStatusesPagedAsync(int page, int pageSize, string? search);
        Task<PropertyStatusViewModel?> GetPropertyStatusByIdAsync(int id);
        Task CreatePropertyStatusAsync(PropertyStatusViewModel model);
        Task UpdatePropertyStatusAsync(PropertyStatusViewModel model);
        Task DeletePropertyStatusAsync(int id);
        Task<int> GetPropertyStatusCounterAsync();
        Task<bool> IsStatusUsedAsync(int id);
        int GetMaxIDAsync();
        ValueTask<IEnumerable<PropertyStatusViewModel>> SearchPropertyStatusAsync(Expression<Func<PropertyStatusViewModel, bool>> predicate);
        Task<IEnumerable<PropertyStatusViewModel>> GetAllPropertyStatusesAsync();
        Task<bool> IsStatusNameUniqueAsync(string statusName, int? excludeId = null);
    }
}