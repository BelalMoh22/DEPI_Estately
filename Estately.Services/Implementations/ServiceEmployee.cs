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
    public class ServiceEmployee : IServiceEmployee
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceEmployee(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -----------------------------------------------------------------
        // PAGED LIST
        // -----------------------------------------------------------------
        public async Task<EmployeesListViewModel> GetEmployeePagedAsync(int page, int pageSize, string? search)
        {
            var employees = await _unitOfWork.EmployeeRepository.ReadAllIncluding(
                "JobTitle",
                "BranchDepartment",
                "BranchDepartment.Branch",
                "BranchDepartment.Department",
                "User"
            );

            var query = employees.Where(e => e.IsDeleted == false).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(e =>
                    (e.FirstName + " " + e.LastName).ToLower().Contains(search)
                    || e.Phone.Contains(search)
                    || (e.JobTitle!.JobTitleName).ToLower().Contains(search)
                );
            }

            int total = query.Count();

            var paged = query.OrderBy(e => e.EmployeeID)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new EmployeesListViewModel
            {
                Employees = paged.Select(ConvertToVM).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                SearchTerm = search
            };
        }

        // -----------------------------------------------------------------
        // GET ALL
        // -----------------------------------------------------------------
        public async Task<EmployeesListViewModel> GetAllEmployeesAsync()
        {
            var list = await _unitOfWork.EmployeeRepository.ReadAllIncluding(
                "JobTitle",
                "BranchDepartment",
                "BranchDepartment.Branch",
                "BranchDepartment.Department",
                "User"
            );

            return new EmployeesListViewModel
            {
                Employees = list
                    .Where(e => e.IsDeleted == false)
                    .Select(ConvertToVM)
                    .ToList()
            };
        }

        // -----------------------------------------------------------------
        // GET BY ID
        // -----------------------------------------------------------------
        public async Task<EmployeesViewModel?> GetEmployeeByIdAsync(int? id)
        {
            if (id == null) return null;

            var employees = await _unitOfWork.EmployeeRepository.ReadAllIncluding(
                "JobTitle",
                "BranchDepartment",
                "BranchDepartment.Branch",
                "BranchDepartment.Department",
                "User"
            );

            var e = employees.FirstOrDefault(x => x.EmployeeID == id && x.IsDeleted == false);
            if (e == null) return null;

            return ConvertToVM(e);
        }

        // -----------------------------------------------------------------
        // UPDATE
        // -----------------------------------------------------------------
        public async Task UpdateEmployeeAsync(EmployeesViewModel vm)
        {
            var entity = await _unitOfWork.EmployeeRepository.GetByIdAsync(vm.EmployeeID);
            if (entity == null) return;

            // Update the existing tracked entity to avoid EF tracking conflicts
            entity.UserID = vm.UserID;
            entity.FirstName = vm.FirstName;
            entity.LastName = vm.LastName;
            entity.Phone = vm.Phone;
            entity.Gender = vm.Gender;
            entity.Age = vm.Age;
            entity.Nationalid = vm.Nationalid;
            entity.JobTitleId = vm.JobTitleId;
            entity.BranchDepartmentId = vm.BranchDepartmentId;
            entity.ReportsTo = vm.ReportsTo;
            entity.Salary = vm.Salary;
            entity.ProfilePhoto = vm.ProfilePhoto;
            entity.HireDate = vm.HireDate;

            await _unitOfWork.EmployeeRepository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        // -----------------------------------------------------------------
        // DELETE (Soft)
        // -----------------------------------------------------------------
        public async Task DeleteEmployeeAsync(int id)
        {
            var entity = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (entity == null) return;

            entity.IsDeleted = true;
            await _unitOfWork.EmployeeRepository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        // -----------------------------------------------------------------
        // COUNTER
        // -----------------------------------------------------------------
        public async Task<int> GetEmployeeCounterAsync()
        {
            var all = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            return all.Count(e => e.IsDeleted == false);
        }

        // -----------------------------------------------------------------
        // MAPPING
        // -----------------------------------------------------------------
        private EmployeesViewModel ConvertToVM(TblEmployee e)
        {
            return new EmployeesViewModel
            {
                EmployeeID = e.EmployeeID,
                UserID = e.UserID,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Phone = e.Phone,
                Gender = e.Gender,
                Age = e.Age,
                Nationalid = e.Nationalid,
                Salary = e.Salary,
                HireDate = e.HireDate,
                JobTitleId = e.JobTitleId,
                BranchDepartmentId = e.BranchDepartmentId,
                ReportsTo = e.ReportsTo,
                Email = e.User?.Email,

                JobTitleName = e.JobTitle?.JobTitleName ?? "",
                BranchName = e.BranchDepartment?.Branch?.BranchName ?? "",
                DepartmentName = e.BranchDepartment?.Department?.DepartmentName ?? ""
            };
        }

        private TblEmployee ConvertToEntity(EmployeesViewModel vm)
        {
            return new TblEmployee
            {
                EmployeeID = vm.EmployeeID,
                UserID = vm.UserID,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Phone = vm.Phone,
                Gender = vm.Gender,
                Age = vm.Age,
                Nationalid = vm.Nationalid,
                JobTitleId = vm.JobTitleId,
                BranchDepartmentId = vm.BranchDepartmentId,
                ReportsTo = vm.ReportsTo,
                Salary = vm.Salary,
                HireDate = vm.HireDate,
                IsActive = true,
                IsDeleted = false
            };
        }
    }
}
