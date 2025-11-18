using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.Interfaces
{
    public interface IServiceZone
    {
        //Task<TblZone> GetZonePagedAsync(int page, int pageSize, string? search);
        //Task<ZonesViewModel?> GetZoneByIdAsync(int id);
        //Task CreateZoneAsync(ZonesViewModel model);
        //Task UpdateZoneAsync(ZonesViewModel model);
        //Task DeleteZoneAsync(int id);
        //Task<int> GetZoneCounterAsync();
        //int GetMaxIDAsync();
        //ValueTask<IEnumerable<TblUser>> SearchZoneAsync(Expression<Func<TblUser, bool>> predicate);

        Task<ZonesListViewModel> GetAllZonesAsync();
        Task<ZonesListViewModel> GetZonePagedAsync(int page, int pageSize, string? search);
        Task<ZonesViewModel?> GetZoneByIdAsync(int? id);
        Task CreateZoneAsync(ZonesViewModel model);
        Task UpdateZoneAsync(ZonesViewModel model);
        Task DeleteZoneAsync(int id);
        Task<int> GetZoneCounterAsync();
        Task<IEnumerable<TblCity>> GetAllCitiesAsync();
        int GetMaxIDAsync();
        ValueTask<IEnumerable<TblZone>> SearchZoneAsync(Expression<Func<TblZone, bool>> predicate);

    }
}
