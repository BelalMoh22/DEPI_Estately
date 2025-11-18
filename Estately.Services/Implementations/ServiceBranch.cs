using Estately.Core.Entities;
using Estately.Infrastructure.UnitOfWorks;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Estately.Services.Implementations
{
    public class ServiceBranch : IServiceBranch
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceBranch(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ================================
        // 1) Get Branch by ID
        // ================================
        public async Task<BranchesViewModel?> GetBranchByIdAsync(int? id)
        {
            var branches = await _unitOfWork.BranchRepository.ReadAllAsync();
            var branch = branches.FirstOrDefault(x => x.BranchID == id);

            return branch == null ? null : ConvertToViewModel(branch);
        }

        // ================================
        // 2) Get paged Branches (with search)
        // ================================
        public async Task<BranchesListViewModel> GetBranchPagedAsync(int page, int pageSize, string? search)
        {
            var branches = await _unitOfWork.BranchRepository.ReadAllAsync();

            var query = branches
                .Where(b => b.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                query = query.Where(b =>
                    (b.BranchName ?? "").ToLower().Contains(search) ||
                    (b.ManagerName ?? "").ToLower().Contains(search)
                );
            }

            int totalCount = query.Count();

            var pagedBranches = query
                .OrderBy(b => b.BranchID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BranchesListViewModel
            {
                Branches = pagedBranches.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = search,
                TotalCount = totalCount
            };
        }

        // ================================
        // 3) Create Branch
        // ================================
        public async Task CreateBranchAsync(BranchesViewModel model)
        {
            var branch = new TblBranch
            {
                BranchName = model.BranchName,
                ManagerName = model.ManagerName,
                Address = model.Address,
                Phone = model.Phone,
                IsDeleted = false
            };

            await _unitOfWork.BranchRepository.AddAsync(branch);
            await _unitOfWork.CompleteAsync();
        }

        // ================================
        // 4) Update Branch
        // ================================
        public async Task UpdateBranchAsync(BranchesViewModel model)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(model.BranchID);
            if (branch == null)
                return;

            // Normal updates
            branch.BranchName = model.BranchName;
            branch.ManagerName = model.ManagerName;
            branch.Address = model.Address;
            branch.Phone = model.Phone;

            branch.IsDeleted = model.IsDeleted;

            await _unitOfWork.BranchRepository.UpdateAsync(branch);
            await _unitOfWork.CompleteAsync();
        }


        // ================================
        // 5) Soft Delete Branch
        // ================================
        public async Task DeleteBranchAsync(int id)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);
            if (branch == null)
                return;

            branch.IsDeleted = true;

            await _unitOfWork.BranchRepository.UpdateAsync(branch);
            await _unitOfWork.CompleteAsync();
        }


        // ================================
        // 6) Counter
        // ================================
        public async Task<int> GetBranchCounterAsync()
        {
            return await _unitOfWork.BranchRepository.CounterAsync();
        }

        // ================================
        // 7) Get all Managers (from Employees)
        // ================================
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

        // ================================
        // 8) Max ID
        // ================================
        public int GetMaxIDAsync()
        {
            return _unitOfWork.BranchRepository.GetMaxId();
        }

        // ================================
        // 9) Search
        // ================================
        public async ValueTask<IEnumerable<TblBranch>> SearchZoneAsync(Expression<Func<TblBranch, bool>> predicate)
        {
            return await _unitOfWork.BranchRepository.Search(predicate);
        }

        // ================================
        // 10) Mapper
        // ================================
        private BranchesViewModel ConvertToViewModel(TblBranch b)
        {
            return new BranchesViewModel
            {
                BranchID = b.BranchID,
                BranchName = b.BranchName,
                ManagerName = b.ManagerName,
                Address = b.Address,
                Phone = b.Phone,
                IsDeleted = b.IsDeleted
            };
        }
    }
}
