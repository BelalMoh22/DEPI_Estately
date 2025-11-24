using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.Interfaces
{
    public interface IServicePropertyFeature
    {
        Task<PropertyFeatureListViewModel> GetPropertyFeaturesPagedAsync(int page, int pageSize, string? search);
        Task<PropertyFeatureViewModel?> GetPropertyFeatureByIdAsync(int id);
        Task CreatePropertyFeatureAsync(PropertyFeatureViewModel model);
        Task UpdatePropertyFeatureAsync(PropertyFeatureViewModel model);
        Task DeletePropertyFeatureAsync(int id);
        Task<int> GetPropertyFeatureCounterAsync();
        Task<bool> IsFeatureUsedAsync(int id);
        int GetMaxID();
        ValueTask<IEnumerable<PropertyFeatureViewModel>> SearchPropertyFeaturesAsync(Expression<Func<PropertyFeatureViewModel, bool>> predicate);
        Task<IEnumerable<PropertyFeatureViewModel>> GetAllPropertyFeaturesAsync();
        Task<bool> IsFeatureNameUniqueAsync(string featureName, int? excludeId = null);
    }
}
