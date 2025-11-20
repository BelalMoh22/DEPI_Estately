using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    public interface IServiceEmployee
    {
        Task<EmployeesListViewModel> GetAllEmployeesAsync();
        Task<EmployeesListViewModel> GetEmployeePagedAsync(int page, int pageSize, string? search);
        Task<EmployeesViewModel?> GetEmployeeByIdAsync(int? id);
        Task UpdateEmployeeAsync(EmployeesViewModel model);
        Task DeleteEmployeeAsync(int id);
        Task<int> GetEmployeeCounterAsync();
        //int GetMaxID();
        //Task<List<TblEmployee>> SearchEmployeeAsync(Expression<Func<TblEmployee, bool>> predicate);
    }
}
