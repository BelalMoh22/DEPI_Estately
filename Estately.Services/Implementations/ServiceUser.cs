using System.Linq.Expressions;

namespace Estately.Services.Implementations
{
    public class ServiceUser : IServiceUser
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceUser(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // 1. LIST USERS (SEARCH + PAGINATION)
        public async Task<UserListViewModel> GetUsersPagedAsync(int page, int pageSize, string? search)
        {
            // Step 1: Load all users with UserType
            var users = await _unitOfWork.UserRepository.ReadAllIncluding("UserType");
            var query = users.AsQueryable();

            // Step 2: Filtering (case-insensitive search)
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();

                query = query.Where(u =>
                    (u.Email ?? "").ToLower().Contains(searchLower) ||
                    (u.Username ?? "").ToLower().Contains(searchLower)
                );
            }

            // Step 3: Total count AFTER FILTER
            int totalCount = query.Count();

            // Step 4: Pagination
            var pagedUsers = query
                .OrderBy(u => u.UserID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Step 5: Build ViewModel
            return new UserListViewModel
            {
                Users = pagedUsers.Select(ConvertToViewModel).ToList(),
                UserTypes = (await _unitOfWork.UserTypeRepository.ReadAllAsync())
                    .Select(ut => new UserTypeViewModel
                    {
                        UserTypeID = ut.UserTypeID,
                        UserTypeName = ut.UserTypeName,
                    }).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = search,
                TotalCount = totalCount
            };
        }

        // ====================================================
        // 2. GET USER BY ID
        // ====================================================
        public async Task<UserViewModel?> GetUserByIdAsync(int id)
        {
            var users = await _unitOfWork.UserRepository.ReadAllIncluding("UserType");
            var user = users.FirstOrDefault(x => x.UserID == id);
            return user == null ? null : ConvertToViewModel(user);
        }

        // 4. UPDATE USER
        public async Task UpdateUserAsync(UserViewModel model)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(model.UserID);
            if (user == null) return;

            user.Email = model.Email;
            user.Username = model.Username;
            user.UserTypeID = model.UserTypeID;
            user.IsEmployee = model.IsEmployee;
            user.IsClient = model.IsClient;
            user.IsDeveloper = model.IsDeveloper;
            user.IsDeleted = model.IsDeleted ?? false;

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 5. DELETE USER (SOFT DELETE)
        // ====================================================
        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return;

            user.IsDeleted = true;

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 6. TOGGLE ACTIVE / INACTIVE
        // ====================================================
        //public async Task ToggleStatusAsync(int id)
        //{
        //    var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        //    if (user == null) return;

        //    user.IsActive = !user.IsActive;

        //    await _unitOfWork.UserRepository.UpdateAsync(user);
        //    await _unitOfWork.CompleteAsync();
        //}

        // ====================================================
        // 7. ASSIGN ROLE
        // ====================================================
        public async Task AssignRoleAsync(int userId, int userTypeId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null) return;

            user.UserTypeID = userTypeId;

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 8. USER COUNTER (STATS)
        // ====================================================
        public async Task<int> GetUserCounterAsync()
        {
            return await _unitOfWork.UserRepository.CounterAsync();
        }

        // ====================================================
        // 9. GET MAX ID
        // ====================================================
        public int GetMaxIDAsync()
        {
            return _unitOfWork.UserRepository.GetMaxId();
        }

        // ====================================================
        // 10. SEARCH USERS
        // ====================================================
        public async ValueTask<IEnumerable<TblUser>> SearchUserAsync(Expression<Func<TblUser, bool>> predicate)
        {
            return await _unitOfWork.UserRepository.Search(predicate);
        }

        public async Task<IEnumerable<UserTypeViewModel>> GetAllUserTypesAsync()
        {
            var types = await _unitOfWork.UserTypeRepository.ReadAllAsync();

            return types.Select(ut => new UserTypeViewModel
            {
                UserTypeID = ut.UserTypeID,
                UserTypeName = ut.UserTypeName
            });
        }

        // ====================================================
        // HELPER: ENTITY -> VIEWMODEL
        // ====================================================
        private UserViewModel ConvertToViewModel(TblUser u)
        {
            return new UserViewModel
            {
                UserID = u.UserID,
                UserTypeID = u.UserTypeID,
                Email = u.Email,
                Username = u.Username,
                IsEmployee = u.IsEmployee,
                IsClient = u.IsClient,
                IsDeveloper = u.IsDeveloper,
                CreatedAt = u.CreatedAt,
                IsDeleted = u.IsDeleted,
                UserTypeName = u.UserType?.UserTypeName
            };
        }
    }
} 