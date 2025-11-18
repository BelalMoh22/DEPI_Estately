using System.Linq.Expressions;


namespace Estately.Services.Interfaces
{
    public interface IServiceProperty
    {
        // 1. Paging + searching
        Task<PropertyListViewModel> GetPropertiesPagedAsync(int page, int pageSize, string? search);

        // 2. Get one property
        Task<PropertyViewModel?> GetPropertyByIdAsync(int id);

        // 3. Create
        Task CreatePropertyAsync(PropertyViewModel model);

        // 4. Update
        Task UpdatePropertyAsync(PropertyViewModel model);

        // 5. Delete (soft)
        Task DeletePropertyAsync(int id);

        // 6. Search
        ValueTask<IEnumerable<TblProperty>> SearchPropertyAsync(Expression<Func<TblProperty, bool>> predicate);

        // 7. Lookups
        Task<IEnumerable<LkpPropertyTypeViewModel>> GetAllPropertyTypesAsync();
        Task<IEnumerable<LkpPropertyStatusViewModel>> GetAllStatusesAsync();
        Task<IEnumerable<DeveloperProfileViewModel>> GetAllDevelopersAsync();
        Task<IEnumerable<ZonesViewModel>> GetAllZonesAsync();

        // Agents lookup
        Task<IEnumerable<TblEmployee>> GetAgentsAsync();

        // Helpers
        int GetMaxID();
        Task<int> GetPropertyCounterAsync();
    }
}
