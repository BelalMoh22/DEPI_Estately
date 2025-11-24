using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.Implementations
{
    public class ServicePropertyFeature : IServicePropertyFeature
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicePropertyFeature(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -----------------------------------------------------------
        // 1. GET PAGED + SEARCH
        // -----------------------------------------------------------
        public async Task<PropertyFeatureListViewModel> GetPropertyFeaturesPagedAsync(int page, int pageSize, string? search)
        {
            var features = await _unitOfWork.PropertyFeatureRepository.ReadAllAsync();
            var query = features.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string s = search.ToLower();
                query = query.Where(f =>
                    (f.FeatureName ?? "").ToLower().Contains(s) ||
                    (f.Description ?? "").ToLower().Contains(s)
                );
            }

            int totalCount = query.Count();

            var paged = query
                .OrderBy(f => f.FeatureID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ConvertToViewModel)
                .ToList();

            return new PropertyFeatureListViewModel
            {
                PropertyFeatures = paged,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = search
            };
        }

        // -----------------------------------------------------------
        // 2. GET BY ID
        // -----------------------------------------------------------
        public async Task<PropertyFeatureViewModel?> GetPropertyFeatureByIdAsync(int id)
        {
            var f = await _unitOfWork.PropertyFeatureRepository.GetByIdAsync(id);
            return f == null ? null : ConvertToViewModel(f);
        }

        // -----------------------------------------------------------
        // 3. CREATE
        // -----------------------------------------------------------
        public async Task CreatePropertyFeatureAsync(PropertyFeatureViewModel model)
        {
            var feature = new TblPropertyFeature
            {
                FeatureName = model.FeatureName,
                Description = model.Description,
                IsAmenity = model.IsAmenity,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.PropertyFeatureRepository.AddAsync(feature);
            await _unitOfWork.CompleteAsync();
        }

        // -----------------------------------------------------------
        // 4. UPDATE
        // -----------------------------------------------------------
        public async Task UpdatePropertyFeatureAsync(PropertyFeatureViewModel model)
        {
            var f = await _unitOfWork.PropertyFeatureRepository.GetByIdAsync(model.FeatureID);
            if (f == null) return;

            f.FeatureName = model.FeatureName;
            f.Description = model.Description;
            f.IsAmenity = model.IsAmenity;

            await _unitOfWork.PropertyFeatureRepository.UpdateAsync(f);
            await _unitOfWork.CompleteAsync();
        }

        // -----------------------------------------------------------
        // 5. DELETE
        // -----------------------------------------------------------
        public async Task DeletePropertyFeatureAsync(int id)
        {
            await _unitOfWork.PropertyFeatureRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        // -----------------------------------------------------------
        // 6. COUNTER
        // -----------------------------------------------------------
        public Task<int> GetPropertyFeatureCounterAsync()
            => _unitOfWork.PropertyFeatureRepository.CounterAsync();

        // -----------------------------------------------------------
        // 7. MAX ID
        // -----------------------------------------------------------
        public int GetMaxID()
            => _unitOfWork.PropertyFeatureRepository.GetMaxId();

        // -----------------------------------------------------------
        // 8. SEARCH
        // -----------------------------------------------------------
        public async ValueTask<IEnumerable<PropertyFeatureViewModel>> SearchPropertyFeaturesAsync(Expression<Func<PropertyFeatureViewModel, bool>> predicate)
        {
            var all = (await _unitOfWork.PropertyFeatureRepository.ReadAllAsync())
                .Select(ConvertToViewModel);

            return all.Where(predicate.Compile());
        }

        // -----------------------------------------------------------
        // 9. GET ALL
        // -----------------------------------------------------------
        public async Task<IEnumerable<PropertyFeatureViewModel>> GetAllPropertyFeaturesAsync()
        {
            return (await _unitOfWork.PropertyFeatureRepository.ReadAllAsync())
                .Select(ConvertToViewModel);
        }

        // -----------------------------------------------------------
        // 10. CHECK UNIQUE
        // -----------------------------------------------------------
        public async Task<bool> IsFeatureNameUniqueAsync(string featureName, int? excludeId = null)
        {
            var all = await _unitOfWork.PropertyFeatureRepository.ReadAllAsync();

            return excludeId.HasValue
                ? !all.Any(f => f.FeatureName.Equals(featureName, StringComparison.OrdinalIgnoreCase)
                             && f.FeatureID != excludeId.Value)
                : !all.Any(f => f.FeatureName.Equals(featureName, StringComparison.OrdinalIgnoreCase));
        }

        // -----------------------------------------------------------
        // 11. CHECK IF FEATURE IS USED
        // -----------------------------------------------------------
        public async Task<bool> IsFeatureUsedAsync(int id)
        {
            var mappings = await _unitOfWork.PropertyFeaturesMappingRepository.Search(m => m.FeatureID == id);
            return mappings.Any();
        }

        // -----------------------------------------------------------
        // HELPER
        // -----------------------------------------------------------
        private PropertyFeatureViewModel ConvertToViewModel(TblPropertyFeature f)
        {
            return new PropertyFeatureViewModel
            {
                FeatureID = f.FeatureID,
                FeatureName = f.FeatureName,
                Description = f.Description,
                IsAmenity = f.IsAmenity,
                CreatedAt = f.CreatedAt
            };
        }
    }
}
