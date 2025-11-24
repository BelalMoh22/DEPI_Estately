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
        Task<bool> CityNameExistsAsync(string cityName, int? cityId);
        //Task<bool> CityNameExistsForEditAsync(string cityName, int cityId);
        Task<int> GetCityCounterAsync();
        int GetMaxIDAsync();
        ValueTask<IEnumerable<TblCity>> SearchCityAsync(Expression<Func<TblCity, bool>> predicate);
        Task<IEnumerable<ZonesViewModel>> GetAllZonesAsync();
    }
}