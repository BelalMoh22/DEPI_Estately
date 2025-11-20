using System.Linq.Expressions;

namespace Estately.Services.Interfaces
{
    public interface IServiceProperty
    {
        // ---------------------------------------------------------
        // 1. Pagination + Search (Images + Features included)
        // ---------------------------------------------------------
        Task<PropertyListViewModel> GetPropertiesPagedAsync(int page, int pageSize, string? search);

        // ---------------------------------------------------------
        // 2. Get Property By ID (Images + Features loaded)
        // ---------------------------------------------------------
        Task<PropertyViewModel?> GetPropertyByIdAsync(int id);

        // ---------------------------------------------------------
        // 3. Create (Images + Features)
        // ---------------------------------------------------------
        Task CreatePropertyAsync(PropertyViewModel model);

        // ---------------------------------------------------------
        // 4. Update (Images + Features)
        // ---------------------------------------------------------
        Task UpdatePropertyAsync(PropertyViewModel model);

        // ---------------------------------------------------------
        // 5. Soft Delete
        // ---------------------------------------------------------
        Task DeletePropertyAsync(int id);

        // ---------------------------------------------------------
        // 6. Search Helper
        // ---------------------------------------------------------
        ValueTask<IEnumerable<TblProperty>> SearchPropertyAsync(Expression<Func<TblProperty, bool>> predicate);

        // ---------------------------------------------------------
        // 7. Features
        // ---------------------------------------------------------
        Task<List<PropertyFeatureViewModel>> GetAllFeaturesAsync();

        // ---------------------------------------------------------
        // 8. Lookups
        // ---------------------------------------------------------
        Task<IEnumerable<LkpPropertyTypeViewModel>> GetAllPropertyTypesAsync();
        Task<IEnumerable<PropertyStatusViewModel>> GetAllStatusesAsync();
        Task<IEnumerable<DeveloperProfileViewModel>> GetAllDevelopersAsync();
        Task<IEnumerable<ZonesViewModel>> GetAllZonesAsync();

        // ---------------------------------------------------------
        // 10. Helper Methods
        // ---------------------------------------------------------
        Task<int> GetMaxIDAsync();
        Task<int> GetPropertyCounterAsync();
    }
}