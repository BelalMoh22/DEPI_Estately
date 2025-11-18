using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    public interface IServiceCity
    {
        Task<CityListViewModel> GetCitiesPagedAsync(int page, int pageSize, string? search);
        Task<CityViewModel?> GetCityByIdAsync(int id);
        Task CreateCityAsync(CityViewModel model);
        Task UpdateCityAsync(CityViewModel model);
        Task DeleteCityAsync(int id);
        Task AssignZoneAsync(int cityId, int zoneId);
        Task<int> GetCityCounterAsync();
        int GetMaxIDAsync();
        ValueTask<IEnumerable<TblCity>> SearchCityAsync(Expression<Func<TblCity, bool>> predicate);
        Task<IEnumerable<ZoneViewModel>> GetAllZonesAsync();
    }
}