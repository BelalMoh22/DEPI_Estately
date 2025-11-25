using Estately.Core.Entities.Identity;
using Estately.Core.Entities;
using Estately.Core.Interfaces;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Estately.Services.Implementations
{
    public class ServiceUser : IServiceUser
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceUser(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ApplicationUserListViewModel> GetUsersPagedAsync(int page, int pageSize, string? search)
        {
            var query = _unitOfWork.UserRepository.Query();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(u =>
                    u.UserName!.ToLower().Contains(search) ||
                    (u.Email != null && u.Email.ToLower().Contains(search))
                );
            }

            int total = query.Count();

            var users = query
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            // Load user types once for name lookup
            var userTypes = await _unitOfWork.UserTypeRepository.ReadAllAsync();
            var userTypeDict = userTypes.ToDictionary(ut => ut.UserTypeID, ut => ut.UserTypeName);

            var list = new ApplicationUserListViewModel
            {
                Users = users.Select(u => ConvertToVM(u, userTypeDict)).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                SearchTerm = search
            };

            return list;
        }

        public async Task<ApplicationUserViewModel?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return null;

            var userTypes = await _unitOfWork.UserTypeRepository.ReadAllAsync();
            var userTypeDict = userTypes.ToDictionary(ut => ut.UserTypeID, ut => ut.UserTypeName);

            return ConvertToVM(user, userTypeDict);
        }

        public async Task UpdateUserAsync(ApplicationUserViewModel model)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(model.Id);
            if (user == null) return;

            var originalUserTypeId = user.UserTypeID;

            user.UserName = model.UserName;
            user.Email = model.Email;

            // If the user type has changed (and neither old nor new type is admin), clean up old profile rows
            if (model.UserTypeID != originalUserTypeId && originalUserTypeId != 4 && model.UserTypeID != 4)
            {
                // Employee profiles
                var employees = await _unitOfWork.EmployeeRepository.Search(e => e.UserID == user.Id);
                foreach (var emp in employees)
                {
                    await _unitOfWork.EmployeeRepository.DeleteAsync(emp.EmployeeID);
                }

                // Client profiles
                var clients = await _unitOfWork.ClientProfileRepository.Search(c => c.UserID == user.Id);
                foreach (var cli in clients)
                {
                    await _unitOfWork.ClientProfileRepository.DeleteAsync(cli.ClientProfileID);
                }

                // Developer profiles
                var developers = await _unitOfWork.DeveloperProfileRepository.Search(d => d.UserID == user.Id);
                foreach (var dev in developers)
                {
                    await _unitOfWork.DeveloperProfileRepository.DeleteAsync(dev.DeveloperProfileID);
                }

                await _unitOfWork.CompleteAsync();
            }

            // After cleanup, create a new profile row appropriate to the NEW type if needed
            if (model.UserTypeID == 2) // Employee
            {
                var existingEmployees = await _unitOfWork.EmployeeRepository.Search(e => e.UserID == user.Id);
                if (!existingEmployees.Any())
                {
                    // pick a valid default JobTitle and (optionally) BranchDepartment so constraints pass
                    var jobTitles = await _unitOfWork.JobTitleRepository.ReadAllAsync();
                    var firstJobTitle = jobTitles.FirstOrDefault();
                    if (model.UserTypeID == 2) 
                    { 
                        var branchDepartments = await _unitOfWork.BranchDepartmentRepository.ReadAllAsync();
                        var firstBranchDept = branchDepartments.FirstOrDefault();

                        var newEmp = new TblEmployee
                        {
                            UserID = user.Id,
                            FirstName = string.Empty,
                            LastName = string.Empty,
                            Gender = "NotSet",
                            JobTitle = firstJobTitle!,
                            Age = 18, // within valid range
                            Phone = string.Empty,
                            Nationalid = "00000000000000", // 14 numeric chars
                            Salary = 0m,
                            JobTitleId = firstJobTitle.JobTitleId,
                            BranchDepartmentId = firstBranchDept?.BranchDepartmentID
                        };

                        await _unitOfWork.EmployeeRepository.AddAsync(newEmp);
                        await _unitOfWork.CompleteAsync();
                    }
                }
            }
            else if (model.UserTypeID == 1) // Client
            {
                var existingClients = await _unitOfWork.ClientProfileRepository.Search(c => c.UserID == user.Id);
                if (!existingClients.Any())
                {
                    var newClient = new TblClientProfile
                    {
                        UserID = user.Id
                    };
                    await _unitOfWork.ClientProfileRepository.AddAsync(newClient);
                    await _unitOfWork.CompleteAsync();
                }
            }
            else if (model.UserTypeID == 3) // Developer
            {
                var existingDevs = await _unitOfWork.DeveloperProfileRepository.Search(d => d.UserID == user.Id);
                if (!existingDevs.Any())
                {
                    var newDev = new TblDeveloperProfile
                    {
                        UserID = user.Id,
                        DeveloperTitle = ""
                    };
                    await _unitOfWork.DeveloperProfileRepository.AddAsync(newDev);
                    await _unitOfWork.CompleteAsync();
                }
            }

            user.UserTypeID = model.UserTypeID;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Let controller handle errors through IdentityResult if needed (for now we just return)
                return;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return;

            // Block deletion for admin users
            if (user.UserTypeID == 4)
            {
                return;
            }

            // Delete related profile entities first (if they exist), based on UserID foreign key

            // Employee profile
            var employees = await _unitOfWork.EmployeeRepository.Search(e => e.UserID == user.Id);
            var employee = employees.FirstOrDefault();
            if (employee != null)
            {
                await _unitOfWork.EmployeeRepository.DeleteAsync(employee.EmployeeID);
            }

            // Client profile
            var clients = await _unitOfWork.ClientProfileRepository.Search(c => c.UserID == user.Id);
            var client = clients.FirstOrDefault();
            if (client != null)
            {
                await _unitOfWork.ClientProfileRepository.DeleteAsync(client.ClientProfileID);
            }

            // Developer profile
            var developers = await _unitOfWork.DeveloperProfileRepository.Search(d => d.UserID == user.Id);
            var developer = developers.FirstOrDefault();
            if (developer != null)
            {
                await _unitOfWork.DeveloperProfileRepository.DeleteAsync(developer.DeveloperProfileID);
            }

            await _unitOfWork.CompleteAsync();

            // Finally delete the identity user
            await _userManager.DeleteAsync(user);
        }

        private ApplicationUserViewModel ConvertToVM(ApplicationUser user, IDictionary<int, string>? userTypeDict = null)
        {
            string? userTypeName = null;
            if (user.UserTypeID.HasValue && userTypeDict != null)
            {
                userTypeDict.TryGetValue(user.UserTypeID.Value, out userTypeName);
            }

            return new ApplicationUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                CreatedAt = user.CreatedAt,
                UserTypeID = user.UserTypeID,
                UserTypeName = userTypeName
            };
        }
    }
}
