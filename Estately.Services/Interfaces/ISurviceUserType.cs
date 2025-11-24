using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    public interface IServiceUserType
    {
        // Get paginated list of user types
        Task<UserTypeListViewModel> GetUserTypesPagedAsync(int page, int pageSize, string? search);

        // Get single user type by ID
        Task<UserTypeViewModel?> GetUserTypeByIdAsync(int id);

        // Create new user type
        Task CreateUserTypeAsync(UserTypeViewModel model);

        // Update existing user type
        Task UpdateUserTypeAsync(UserTypeViewModel model);

        // Delete user type
        Task DeleteUserTypeAsync(int id);

        // Toggle status if you have an IsActive field (commented out as it wasn't in your entity)
        // Task ToggleStatusAsync(int id);

        // Get total count of user types
        Task<int> GetUserTypeCounterAsync();

        // Get maximum UserTypeID
        int GetMaxIDAsync();

        // Search user types with predicate
        ValueTask<IEnumerable<UserTypeViewModel>> SearchUserTypeAsync(Expression<Func<UserTypeViewModel, bool>> predicate);

        // Get all user types (for dropdowns, etc.)
        Task<IEnumerable<UserTypeViewModel>> GetAllUserTypesAsync();

        // Additional method that might be useful for your UserType entity
        Task<bool> IsUserTypeNameUniqueAsync(string userTypeName, int? excludeId = null);
        Task<bool> IsTypeUsedAsync(int typeId);

    }
}