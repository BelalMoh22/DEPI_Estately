using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    public interface IServicePropertyType
    {
        Task<PropertyTypeListViewModel> GetPropertyTypesPagedAsync(int page, int pageSize, string? search);
        Task<PropertyTypeViewModel?> GetPropertyTypeByIdAsync(int id);
        Task CreatePropertyTypeAsync(PropertyTypeViewModel model);
        Task UpdatePropertyTypeAsync(PropertyTypeViewModel model);
        Task DeletePropertyTypeAsync(int id);
        Task<int> GetPropertyTypeCounterAsync();
        int GetMaxIDAsync();
        ValueTask<IEnumerable<LkpPropertyType>> SearchPropertyTypeAsync(Expression<Func<LkpPropertyType, bool>> predicate);
    }
}