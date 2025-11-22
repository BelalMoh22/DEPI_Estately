using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Estately.Services.Implementations
{
    public class ServiceCity : IServiceCity
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceCity(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CityListViewModel> GetCitiesPagedAsync(int page, int pageSize, string? search)
        {
            var cities = await _unitOfWork.CityRepository.ReadAllIncluding("TblZones");
            var query = cities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();
                query = query.Where(c => (c.CityName ?? "").ToLower().Contains(searchLower));
            }

            int totalCount = query.Count();

            var pagedCities = query
                .OrderBy(c => c.CityID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new CityListViewModel
            {
                Cities = pagedCities.Select(ConvertToViewModel).ToList(),
                Zones = (await _unitOfWork.ZoneRepository.ReadAllAsync())
                    .Select(z => new ZonesViewModel
                    {
                        ZoneId = z.ZoneID,
                        ZoneName = z.ZoneName,
                    }).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = search,
                TotalCount = totalCount
            };
        }

        public async Task<CityViewModel?> GetCityByIdAsync(int id)
        {
            var cities = await _unitOfWork.CityRepository.ReadAllIncluding("TblZones");
            var city = cities.FirstOrDefault(x => x.CityID == id);
            return city == null ? null : ConvertToViewModel(city);
        }

        public async Task CreateCityAsync(CityViewModel model)
        {
            var city = new TblCity
            {
                CityName = model.CityName,

            };

            await _unitOfWork.CityRepository.AddAsync(city);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateCityAsync(CityViewModel model)
        {
            var city = await _unitOfWork.CityRepository.GetByIdAsync(model.CityID);
            if (city == null) return;

            city.CityName = model.CityName;
            

            await _unitOfWork.CityRepository.UpdateAsync(city);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteCityAsync(int id)
        {
            var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
            if (city == null) return;

            
            await _unitOfWork.CityRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task AssignZoneAsync(int cityId, int zoneId)
        {
            var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
            if (city == null) return;

            // Implementation depends on your relationship structure
            await _unitOfWork.CityRepository.UpdateAsync(city);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<int> GetCityCounterAsync()
        {
            return await _unitOfWork.CityRepository.CounterAsync();
        }

        public int GetMaxIDAsync()
        {
            return _unitOfWork.CityRepository.GetMaxId();
        }

        public async ValueTask<IEnumerable<TblCity>> SearchCityAsync(Expression<Func<TblCity, bool>> predicate)
        {
            return await _unitOfWork.CityRepository.Search(predicate);
        }

        public async Task<IEnumerable<ZonesViewModel>> GetAllZonesAsync()
        {
            var zones = await _unitOfWork.ZoneRepository.ReadAllAsync();
            return zones.Select(z => new ZonesViewModel
            {
                ZoneId = z.ZoneID,
                ZoneName = z.ZoneName,
                CityId = z.CityID,
                City = z.City?.CityName // Assuming you have navigation property
            });
        }

        private CityViewModel ConvertToViewModel(TblCity c)
        {
            return new CityViewModel
            {
                CityID = c.CityID,
                CityName = c.CityName,
                ZoneCount = c.TblZones?.Count ?? 0
            };
        }

        public async Task<bool> CityNameExistsAsync(string cityName)
        {
            var existingCities = await _unitOfWork.CityRepository.Search(
                c => c.CityName.ToLower() == cityName.ToLower());

            return existingCities.Any();
        }
    }
}