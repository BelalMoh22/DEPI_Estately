using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using System;
using System.Linq.Expressions;

namespace Estately.Services.Implementations
{
    public class ServicePropertyStatus : IServicePropertyStatus
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicePropertyStatus(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ====================================================
        // 1. LIST PROPERTY STATUSES (SEARCH + PAGINATION)
        // ====================================================
        public async Task<PropertyStatusListViewModel> GetPropertyStatusesPagedAsync(int page, int pageSize, string? search)
        {
            // Step 1: Load all property statuses
            var propertyStatuses = await _unitOfWork.PropertyStatusRepository.ReadAllAsync();
            var query = propertyStatuses.AsQueryable();

            // Step 2: Filtering (case-insensitive search)
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();

                query = query.Where(ps =>
                    (ps.StatusName ?? "").ToLower().Contains(searchLower) ||
                    (ps.Description ?? "").ToLower().Contains(searchLower)
                );
            }

            // Step 3: Total count AFTER FILTER
            int totalCount = query.Count();

            // Step 4: Pagination
            var pagedStatuses = query
                .OrderBy(ps => ps.StatusID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Step 5: Build ViewModel
            return new PropertyStatusListViewModel
            {
                PropertyStatuses = pagedStatuses.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = search,
                TotalCount = totalCount
            };
        }

        // ====================================================
        // 2. GET PROPERTY STATUS BY ID
        // ====================================================
        public async Task<PropertyStatusViewModel?> GetPropertyStatusByIdAsync(int id)
        {
            var propertyStatus = await _unitOfWork.PropertyStatusRepository.GetByIdAsync(id);
            return propertyStatus == null ? null : ConvertToViewModel(propertyStatus);
        }

        // ====================================================
        // 3. CREATE PROPERTY STATUS
        // ====================================================
        public async Task CreatePropertyStatusAsync(PropertyStatusViewModel model)
        {
            var propertyStatus = new LkpPropertyStatus
            {
                StatusName = model.StatusName,
                Description = model.Description
                // Add other fields as needed based on your entity
            };

            await _unitOfWork.PropertyStatusRepository.AddAsync(propertyStatus);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 4. UPDATE PROPERTY STATUS
        // ====================================================
        public async Task UpdatePropertyStatusAsync(PropertyStatusViewModel model)
        {
            var propertyStatus = await _unitOfWork.PropertyStatusRepository.GetByIdAsync(model.StatusID);
            if (propertyStatus == null) return;

            propertyStatus.StatusName = model.StatusName;
            propertyStatus.Description = model.Description;
            // Update other fields as needed

            await _unitOfWork.PropertyStatusRepository.UpdateAsync(propertyStatus);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 5. DELETE PROPERTY STATUS
        // ====================================================
        public async Task DeletePropertyStatusAsync(int id)
        {
            var propertyStatus = await _unitOfWork.PropertyStatusRepository.GetByIdAsync(id);
            if (propertyStatus == null) return;

            // Soft delete if you have IsDeleted field, otherwise hard delete
            // propertyStatus.IsDeleted = true;

            await _unitOfWork.PropertyStatusRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 6. TOGGLE ACTIVE / INACTIVE (if applicable)
        // ====================================================
        //public async Task ToggleStatusAsync(int id)
        //{
        //    var propertyStatus = await _unitOfWork.PropertyStatusRepository.GetByIdAsync(id);
        //    if (propertyStatus == null) return;

        //    propertyStatus.IsActive = !propertyStatus.IsActive;

        //    await _unitOfWork.PropertyStatusRepository.UpdateAsync(propertyStatus);
        //    await _unitOfWork.CompleteAsync();
        //}

        // ====================================================
        // 7. GET PROPERTY STATUS COUNTER (STATS)
        // ====================================================
        public async Task<int> GetPropertyStatusCounterAsync()
        {
            return await _unitOfWork.PropertyStatusRepository.CounterAsync();
        }

        // ====================================================
        // 8. GET MAX ID
        // ====================================================
        public int GetMaxIDAsync()
        {
            return _unitOfWork.PropertyStatusRepository.GetMaxId();
        }

        // ====================================================
        // 9. SEARCH PROPERTY STATUSES
        // ====================================================
        public async ValueTask<IEnumerable<PropertyStatusViewModel>> SearchPropertyStatusAsync(Expression<Func<PropertyStatusViewModel, bool>> predicate)
        {
            var allStatuses = await _unitOfWork.PropertyStatusRepository.ReadAllAsync();
            var viewModels = allStatuses.Select(ConvertToViewModel);
            return viewModels.Where(predicate.Compile());
        }

        // ====================================================
        // 10. GET ALL PROPERTY STATUSES (for dropdowns, etc.)
        // ====================================================
        public async Task<IEnumerable<PropertyStatusViewModel>> GetAllPropertyStatusesAsync()
        {
            var statuses = await _unitOfWork.PropertyStatusRepository.ReadAllAsync();
            return statuses.Select(ConvertToViewModel);
        }

        // ====================================================
        // 11. CHECK IF STATUS NAME IS UNIQUE
        // ====================================================
        public async Task<bool> IsStatusNameUniqueAsync(string statusName, int? excludeId = null)
        {
            var allStatuses = await _unitOfWork.PropertyStatusRepository.ReadAllAsync();

            if (excludeId.HasValue)
            {
                return !allStatuses.Any(ps =>
                    ps.StatusName.Equals(statusName, StringComparison.OrdinalIgnoreCase) &&
                    ps.StatusID != excludeId.Value);
            }

            return !allStatuses.Any(ps =>
                ps.StatusName.Equals(statusName, StringComparison.OrdinalIgnoreCase));
        }
        public async Task<bool> IsStatusUsedAsync(int id)
        {
            var properties = await _unitOfWork.PropertyRepository
                .Search(p => p.StatusId == id);

            return properties.Any();
        }


        // ====================================================
        // HELPER: ENTITY -> VIEWMODEL
        // ====================================================
        private PropertyStatusViewModel ConvertToViewModel(LkpPropertyStatus ps)
        {
            return new PropertyStatusViewModel
            {
                StatusID = ps.StatusID,
                StatusName = ps.StatusName,
                Description = ps.Description
                // Map other properties as needed
            };
        }
    }
}