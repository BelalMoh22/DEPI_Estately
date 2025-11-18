using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.Implementations
{
    public class ServiceDepartment : IServiceDepartment
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceDepartment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DepartmentsViewModel?> GetDepartmentByIdAsync(int? id)
        {
            var deps = await _unitOfWork.DepartmentRepository.ReadAllAsync();
            var dep = deps.FirstOrDefault(x => x.DepartmentID == id);
            return dep == null ? null : ConvertToViewModel(dep);
        }
        public async Task<DepartmentsListViewModel> GetDepartmentPagedAsync(int page, int pageSize, string? searchTerm)
        {
            // Load Departments
            var deps = await _unitOfWork.DepartmentRepository.ReadAllAsync();

            // EXCLUDE SOFT-DELETED DEPARTMENTS
            var query = deps
                .Where(z => z.IsDeleted == false)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                query = query.Where(d =>
                    (d.DepartmentName ?? "").ToLower().Contains(searchTerm) ||
                    (d.ManagerName ?? "").ToLower().Contains(searchTerm)
                );
            }

            // Count
            int totalCount = query.Count();

            // Paging
            var pagedDeps = query
                .OrderBy(d => d.DepartmentID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return ViewModel
            return new DepartmentsListViewModel
            {
                Departments = pagedDeps.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                TotalCount = totalCount
            };
        }
        public async Task CreateDepartmentAsync(DepartmentsViewModel model)
        {
            var dep = new TblDepartment
            {
                DepartmentName = model.DepartmentName,
                ManagerName = model.ManagerName,
                Email = model.Email,
                IsDeleted = false
            };

            await _unitOfWork.DepartmentRepository.AddAsync(dep);
            await _unitOfWork.CompleteAsync();
        }
        public async Task UpdateDepartmentAsync(DepartmentsViewModel model)
        {
            var dep = await _unitOfWork.DepartmentRepository.GetByIdAsync(model.DepartmentID);
            if (dep == null) return;

            dep.DepartmentName = model.DepartmentName;
            dep.ManagerName = model.ManagerName;

            await _unitOfWork.DepartmentRepository.UpdateAsync(dep);
            await _unitOfWork.CompleteAsync();
        }
        public async Task DeleteDepartmentAsync(int id)
        {
            var dep = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (dep == null) return;

            dep.IsDeleted = true;

            await _unitOfWork.DepartmentRepository.UpdateAsync(dep);
            await _unitOfWork.CompleteAsync();
        }
        public async Task<int> GetDepartmentCounterAsync()
        {
            return await _unitOfWork.DepartmentRepository.CounterAsync();
        }
        public async Task<IEnumerable<TblEmployee>> GetAllManagersAsync()
        {
            return await _unitOfWork.EmployeeRepository
                .Query()
                .Include(e => e.JobTitle)
                .Where(e =>
                    e.IsDeleted == false &&
                    e.JobTitle != null &&
                    e.JobTitle.JobTitleName.Contains("Manager")
                )
                .ToListAsync();
        }
        public int GetMaxIDAsync()
        {
            return _unitOfWork.DepartmentRepository.GetMaxId();
        }
        public async ValueTask<IEnumerable<TblDepartment>> SearchDepartmentAsync(Expression<Func<TblDepartment, bool>> predicate)
        {
            return await _unitOfWork.DepartmentRepository.Search(predicate);
        }
        private DepartmentsViewModel ConvertToViewModel(TblDepartment d)
        {
            return new DepartmentsViewModel
            {
                DepartmentID = d.DepartmentID,
                DepartmentName = d.DepartmentName,
                ManagerName = d.ManagerName,
                Email = d.Email,
                IsDeleted = d.IsDeleted
            };
        }
    }
}
