using System.Linq.Expressions;

namespace Estately.Services.Implementations
{
    public class ServicePropertyType : IServicePropertyType
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicePropertyType(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 1. LIST PROPERTY TYPES (SEARCH + PAGINATION)
        public async Task<PropertyTypeListViewModel> GetPropertyTypesPagedAsync(int page, int pageSize, string? search)
        {
            // Step 1: Load all property types
            var propertyTypes = await _unitOfWork.PropertyTypeRepository.ReadAllAsync();
            var query = propertyTypes.AsQueryable();

            // Step 2: Filtering (case-insensitive search)
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();

                query = query.Where(pt =>
                    (pt.TypeName ?? "").ToLower().Contains(searchLower) ||
                    (pt.Description ?? "").ToLower().Contains(searchLower)
                );
            }

            // Step 3: Total count AFTER FILTER
            int totalCount = query.Count();

            // Step 4: Pagination
            var pagedPropertyTypes = query
                .OrderBy(pt => pt.PropertyTypeID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Step 5: Build ViewModel
            return new PropertyTypeListViewModel
            {
                PropertyTypes = pagedPropertyTypes.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = search,
                TotalCount = totalCount
            };
        }

        // ====================================================
        // 2. GET PROPERTY TYPE BY ID
        // ====================================================
        public async Task<PropertyTypeViewModel?> GetPropertyTypeByIdAsync(int id)
        {
            var propertyTypes = await _unitOfWork.PropertyTypeRepository.ReadAllAsync();
            var propertyType = propertyTypes.FirstOrDefault(x => x.PropertyTypeID == id);
            return propertyType == null ? null : ConvertToViewModel(propertyType);
        }

        // ====================================================
        // 3. CREATE PROPERTY TYPE
        // ====================================================
        public async Task CreatePropertyTypeAsync(PropertyTypeViewModel model)
        {
            var propertyType = new LkpPropertyType
            {
                TypeName = model.TypeName,
                Description = model.Description
            };

            await _unitOfWork.PropertyTypeRepository.AddAsync(propertyType);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 4. UPDATE PROPERTY TYPE
        // ====================================================
        public async Task UpdatePropertyTypeAsync(PropertyTypeViewModel model)
        {
            var propertyType = await _unitOfWork.PropertyTypeRepository.GetByIdAsync(model.PropertyTypeID);
            if (propertyType == null) return;

            propertyType.TypeName = model.TypeName;
            propertyType.Description = model.Description;

            await _unitOfWork.PropertyTypeRepository.UpdateAsync(propertyType);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 5. DELETE PROPERTY TYPE
        // ====================================================
        public async Task DeletePropertyTypeAsync(int id)
        {
            var propertyType = await _unitOfWork.PropertyTypeRepository.GetByIdAsync(id);
            if (propertyType == null) return;

            await _unitOfWork.PropertyTypeRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 6. PROPERTY TYPE COUNTER (STATS)
        // ====================================================
        public async Task<int> GetPropertyTypeCounterAsync()
        {
            return await _unitOfWork.PropertyTypeRepository.CounterAsync();
        }

        // ====================================================
        // 7. GET MAX ID
        // ====================================================
        public int GetMaxIDAsync()
        {
            return _unitOfWork.PropertyTypeRepository.GetMaxId();
        }

        // ====================================================
        // 8. SEARCH PROPERTY TYPES
        // ====================================================
        public async ValueTask<IEnumerable<LkpPropertyType>> SearchPropertyTypeAsync(Expression<Func<LkpPropertyType, bool>> predicate)
        {
            return await _unitOfWork.PropertyTypeRepository.Search(predicate);
        }

        // ====================================================
        // HELPER: ENTITY -> VIEWMODEL
        // ====================================================
        private PropertyTypeViewModel ConvertToViewModel(LkpPropertyType pt)
        {
            return new PropertyTypeViewModel
            {
                PropertyTypeID = pt.PropertyTypeID,
                TypeName = pt.TypeName,
                Description = pt.Description
            };
        }
    }
}