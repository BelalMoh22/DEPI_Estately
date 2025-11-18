using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.Interfaces
{
    public interface IServiceDepartment
    {
        Task<DepartmentsListViewModel> GetDepartmentPagedAsync(int page, int pageSize, string? search);
        Task<DepartmentsViewModel?> GetDepartmentByIdAsync(int? id);
        Task CreateDepartmentAsync(DepartmentsViewModel model);
        Task UpdateDepartmentAsync(DepartmentsViewModel model);
        Task DeleteDepartmentAsync(int id);
        Task<IEnumerable<TblEmployee>> GetAllManagersAsync();
        Task<int> GetDepartmentCounterAsync();
        int GetMaxIDAsync();
        ValueTask<IEnumerable<TblDepartment>> SearchDepartmentAsync(Expression<Func<TblDepartment, bool>> predicate);
    }
}
