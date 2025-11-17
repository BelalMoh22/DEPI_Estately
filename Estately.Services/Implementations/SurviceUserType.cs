using System.Linq.Expressions;
using Estately.Core.Entities;
using Estately.Core.Interfaces;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;

namespace Estately.Services.Implementations
{
    public class ServiceUserType : IServiceUserType
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceUserType(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ====================================================
        // 1. LIST USER TYPES (SEARCH + PAGINATION)
        // ====================================================
        public async Task<UserTypeListViewModel> GetUserTypesPagedAsync(int page, int pageSize, string? search)
        {
            // Step 1: Load all user types
            var userTypes = await _unitOfWork.UserTypeRepository.ReadAllAsync();
            var query = userTypes.AsQueryable();

            // Step 2: Filtering (case-insensitive search)
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();

                query = query.Where(ut =>
                    (ut.UserTypeName ?? "").ToLower().Contains(searchLower) ||
                    (ut.Description ?? "").ToLower().Contains(searchLower)
                );
            }

            // Step 3: Total count AFTER FILTER
            int totalCount = query.Count();

            // Step 4: Pagination
            var pagedUserTypes = query
                .OrderBy(ut => ut.UserTypeID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Step 5: Build ViewModel
            return new UserTypeListViewModel
            {
                UserTypes = pagedUserTypes.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = search,
                TotalCount = totalCount
            };
        }

        // ====================================================
        // 2. GET USER TYPE BY ID
        // ====================================================
        public async Task<UserTypeViewModel?> GetUserTypeByIdAsync(int id)
        {
            var userType = await _unitOfWork.UserTypeRepository.GetByIdAsync(id);
            return userType == null ? null : ConvertToViewModel(userType);
        }

        // ====================================================
        // 3. CREATE USER TYPE
        // ====================================================
        public async Task CreateUserTypeAsync(UserTypeViewModel model)
        {
            var userType = new LkpUserType
            {
                UserTypeName = model.UserTypeName,
                Description = model.Description
            };

            await _unitOfWork.UserTypeRepository.AddAsync(userType);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 4. UPDATE USER TYPE
        // ====================================================
        public async Task UpdateUserTypeAsync(UserTypeViewModel model)
        {
            var userType = await _unitOfWork.UserTypeRepository.GetByIdAsync(model.UserTypeID);
            if (userType == null) return;

            userType.UserTypeName = model.UserTypeName;
            userType.Description = model.Description;

            await _unitOfWork.UserTypeRepository.UpdateAsync(userType);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 5. DELETE USER TYPE
        // ====================================================
        public async Task DeleteUserTypeAsync(int id)
        {
            var userType = await _unitOfWork.UserTypeRepository.GetByIdAsync(id);
            if (userType == null) return;

            // Check if there are users associated with this user type
            var usersWithThisType = await _unitOfWork.UserRepository.Search(u => u.UserTypeID == id);

            if (usersWithThisType.Any())
            {
                throw new InvalidOperationException(
                    $"Cannot delete user type. There are {usersWithThisType.Count()} users associated with this type.");
            }

            await _unitOfWork.UserTypeRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 6. USER TYPE COUNTER (STATS)
        // ====================================================
        public async Task<int> GetUserTypeCounterAsync()
        {
            return await _unitOfWork.UserTypeRepository.CounterAsync();
        }

        // ====================================================
        // 7. GET MAX ID
        // ====================================================
        public int GetMaxIDAsync()
        {
            return _unitOfWork.UserTypeRepository.GetMaxId();
        }

        // ====================================================
        // 8. SEARCH USER TYPES
        // ====================================================
        public async ValueTask<IEnumerable<UserTypeViewModel>> SearchUserTypeAsync(Expression<Func<UserTypeViewModel, bool>> predicate)
        {
            // Since predicate is on ViewModel, we need to convert to entity predicate
            var allUserTypes = await _unitOfWork.UserTypeRepository.ReadAllAsync();
            var viewModels = allUserTypes.Select(ConvertToViewModel);
            return viewModels.Where(predicate.Compile());
        }

        // ====================================================
        // 9. GET ALL USER TYPES (for dropdowns, etc.)
        // ====================================================
        public async Task<IEnumerable<UserTypeViewModel>> GetAllUserTypesAsync()
        {
            var userTypes = await _unitOfWork.UserTypeRepository.ReadAllAsync();
            return userTypes.Select(ConvertToViewModel);
        }

        // ====================================================
        // 10. CHECK IF USER TYPE NAME IS UNIQUE
        // ====================================================
        public async Task<bool> IsUserTypeNameUniqueAsync(string userTypeName, int? excludeId = null)
        {
            var existing = await _unitOfWork.UserTypeRepository.Search(ut =>
                ut.UserTypeName.ToLower() == userTypeName.ToLower());

            if (excludeId.HasValue)
            {
                existing = existing.Where(ut => ut.UserTypeID != excludeId.Value);
            }

            return !existing.Any();
        }

        // ====================================================
        // HELPER: ENTITY -> VIEWMODEL
        // ====================================================
        private UserTypeViewModel ConvertToViewModel(LkpUserType ut)
        {
            return new UserTypeViewModel
            {
                UserTypeID = ut.UserTypeID,
                UserTypeName = ut.UserTypeName,
                Description = ut.Description
            };
        }
    }
}