using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    public interface IServicePropertyStatus
    {
        // Get paginated list of property statuses
        Task<PropertyStatusListViewModel> GetPropertyStatusesPagedAsync(int page, int pageSize, string? search);

        // Get single property status by ID
        Task<PropertyStatusViewModel?> GetPropertyStatusByIdAsync(int id);

        // Create new property status
        Task CreatePropertyStatusAsync(PropertyStatusViewModel model);

        // Update existing property status
        Task UpdatePropertyStatusAsync(PropertyStatusViewModel model);

        // Delete property status
        Task DeletePropertyStatusAsync(int id);

        // Toggle status if you have an IsActive field
        // Task ToggleStatusAsync(int id);

        // Get total count of property statuses
        Task<int> GetPropertyStatusCounterAsync();

        // Get maximum StatusID
        int GetMaxIDAsync();

        // Search property statuses with predicate
        ValueTask<IEnumerable<PropertyStatusViewModel>> SearchPropertyStatusAsync(Expression<Func<PropertyStatusViewModel, bool>> predicate);

        // Get all property statuses (for dropdowns, etc.)
        Task<IEnumerable<PropertyStatusViewModel>> GetAllPropertyStatusesAsync();

        // Additional method that might be useful for your PropertyStatus entity
        Task<bool> IsStatusNameUniqueAsync(string statusName, int? excludeId = null);
    }
}