using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.Implementations
{
    public class ServiceZone : IServiceZone
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceZone(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 1. LIST ZONES (SEARCH + PAGINATION)
        public async Task<ZonesListViewModel> GetZonePagedAsync(int page, int pageSize, string? searchTerm)
        {
            // Load Zones including City
            var zones = await _unitOfWork.ZoneRepository.ReadAllIncluding("City");

            // EXCLUDE SOFT-DELETED ZONES
            var query = zones
                .Where(z => z.IsDeleted == false)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                query = query.Where(z =>
                    (z.ZoneName ?? "").ToLower().Contains(searchTerm) ||
                    (z.City != null && z.City.CityName.ToLower().Contains(searchTerm))
                );
            }

            // Count
            int totalCount = query.Count();

            // Paging
            var pagedZones = query
                .OrderBy(z => z.ZoneID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return ViewModel
            return new ZonesListViewModel
            {
                Zones = pagedZones.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                TotalCount = totalCount
            };
        }

        // ====================================================
        // 2. GET ZONE BY ID
        // ====================================================
        public async Task<ZonesViewModel?> GetZoneByIdAsync(int? id)
        {
            var zones = await _unitOfWork.ZoneRepository.ReadAllIncluding("City");
            var zone = zones.FirstOrDefault(x => x.ZoneID == id);
            return zone == null ? null : ConvertToViewModel(zone);
        }

        // ====================================================
        // 3. CREATE ZONE
        // ====================================================
        public async Task CreateZoneAsync(ZonesViewModel model)
        {
            var zone = new TblZone
            {
                CityID = model.CityId,
                ZoneName = model.ZoneName,
                IsDeleted = false
            };

            await _unitOfWork.ZoneRepository.AddAsync(zone);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 4. UPDATE ZONE
        // ====================================================
        public async Task UpdateZoneAsync(ZonesViewModel model)
        {
            var zone = await _unitOfWork.ZoneRepository.GetByIdAsync(model.ZoneId);
            if (zone == null) return;

            zone.CityID = model.CityId;
            zone.ZoneName = model.ZoneName;

            await _unitOfWork.ZoneRepository.UpdateAsync(zone);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 5. DELETE ZONE (SOFT DELETE)
        // ====================================================
        public async Task DeleteZoneAsync(int id)
        {
            var zone = await _unitOfWork.ZoneRepository.GetByIdAsync(id);
            if (zone == null) return;

            zone.IsDeleted = true;

            await _unitOfWork.ZoneRepository.UpdateAsync(zone);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 6. ZONE COUNTER (STATS)
        // ====================================================
        public async Task<int> GetZoneCounterAsync()
        {
            return await _unitOfWork.ZoneRepository.CounterAsync();
        }


        // ====================================================
        // 7. GET ALL CITIES
        // ====================================================
        public async Task<IEnumerable<TblCity>> GetAllCitiesAsync()
        {
            return await _unitOfWork.CityRepository.ReadAllAsync();
        }

        // ====================================================
        // 8. GET MAX ID
        // ====================================================
        public int GetMaxIDAsync()
        {
            return _unitOfWork.ZoneRepository.GetMaxId();
        }

        // ====================================================
        // 9. SEARCH ZONES
        // ====================================================
        public async ValueTask<IEnumerable<TblZone>> SearchZoneAsync(Expression<Func<TblZone, bool>> predicate)
        {
            return await _unitOfWork.ZoneRepository.Search(predicate);
        }

        // ====================================================
        // HELPER: ENTITY -> VIEWMODEL
        // ====================================================
        private ZonesViewModel ConvertToViewModel(TblZone z)
        {
            return new ZonesViewModel
            {
                ZoneId = z.ZoneID,
                CityId = z.CityID,
                ZoneName = z.ZoneName,
                City = z.City?.CityName
            };
        }
    }
}
