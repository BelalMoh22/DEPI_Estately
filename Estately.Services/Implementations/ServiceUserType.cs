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
            // Use EF IQueryable instead of loading everything
            var query = _unitOfWork.UserTypeRepository.Query();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();

                query = query.Where(ut =>
                    (ut.UserTypeName ?? "").ToLower().Contains(searchLower) ||
                    (ut.Description ?? "").ToLower().Contains(searchLower)
                );
            }

            // Count (AFTER filter)
            int totalCount = await query.CountAsync();

            // Pagination
            var pagedUserTypes = await query
                .OrderBy(ut => ut.UserTypeID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Build ViewModel
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
            // 1️⃣ Get the user type
            var userType = await _unitOfWork.UserTypeRepository.GetByIdAsync(id);
            if (userType == null)
                return;

            // 2️⃣ Check if any ApplicationUser uses this UserTypeID
            var usersWithType = await _unitOfWork.UserRepository
                .Query()
                .Where(u => u.UserTypeID == id)
                .ToListAsync();

            // 3️⃣ If users exist → block delete
            if (usersWithType.Any())
            {
                throw new InvalidOperationException(
                    $"Cannot delete user type. There are {usersWithType.Count} users associated with this type.");
            }

            // 4️⃣ Safe delete
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
        public async Task<bool> IsUserTypeNameUniqueAsync(string userTypeName, int? userTypeId)
        {
            var exists = await _unitOfWork.UserTypeRepository.Search(ut =>
                ut.UserTypeName.ToLower() == userTypeName.ToLower()
                && (userTypeId == null || ut.UserTypeID != userTypeId.Value));

            return !exists.Any();
        }

        public async Task<bool> IsTypeUsedAsync(int typeId)
        {
            var properties = await _unitOfWork.UserRepository
                .Search(p => p.UserTypeID == typeId);

            return properties.Any();
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