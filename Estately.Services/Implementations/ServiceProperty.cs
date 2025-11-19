using System.Linq.Expressions;
namespace Estately.Services.Implementations
{
    public class ServiceProperty : IServiceProperty
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProperty(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ---------------------------------------------------------
        // 1. Paging with Search + Images + Features
        // ---------------------------------------------------------
        public async Task<PropertyListViewModel> GetPropertiesPagedAsync(int page, int pageSize, string? search)
        {
            var properties = await _unitOfWork.PropertyRepository.ReadAllIncluding(
                "DeveloperProfile",
                "PropertyType",
                "Status",
                "Zone",
                "TblPropertyImages",
                "TblPropertyFeaturesMappings"
            );

            var query = properties.Where(p => p.IsDeleted == false).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string s = search.ToLower();

                query = query.Where(p =>
                    (p.Address ?? "").ToLower().Contains(s) ||
                    (p.PropertyCode ?? "").ToLower().Contains(s) ||
                    (p.DeveloperProfile!.DeveloperTitle ?? "").ToLower().Contains(s)
                );
            }

            int totalCount = query.Count();

            var paged = query
                .OrderBy(p => p.PropertyID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PropertyListViewModel
            {
                Properties = paged.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = search,
                TotalCount = totalCount,
                Features = await GetAllFeaturesAsync()
            };
        }

        // ---------------------------------------------------------
        // 2. Get By ID
        // ---------------------------------------------------------
        public async Task<PropertyViewModel?> GetPropertyByIdAsync(int id)
        {
            var props = await _unitOfWork.PropertyRepository.ReadAllIncluding(
                "DeveloperProfile",
                "PropertyType",
                "Status",
                "Zone",
                "TblPropertyImages",
                "TblPropertyFeaturesMappings"
            );

            var p = props.FirstOrDefault(x => x.PropertyID == id && x.IsDeleted == false);
            if (p == null) return null;

            var vm = ConvertToViewModel(p);

            vm.AllFeatures = await GetAllFeaturesAsync();
            vm.SelectedFeatures = p.TblPropertyFeaturesMappings.Select(f => f.FeatureID).ToList();

            return vm;
        }

        // ---------------------------------------------------------
        // 3. CREATE Property
        // ---------------------------------------------------------
        public async Task CreatePropertyAsync(PropertyViewModel model)
        {
            var entity = ConvertToEntity(model);
            entity.PropertyCode = ""; // temporarily empty

            await _unitOfWork.PropertyRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            // Create property code
            var zone = await _unitOfWork.ZoneRepository.GetByIdAsync(entity.ZoneID);

            // Remove spaces from ZoneName (optional)
            string zoneName = zone?.ZoneName?.Replace(" ", "") ?? "Unknown";

            entity.PropertyCode = $"PROP-{zoneName}-{entity.PropertyID:D4}";
            // ------------------ IMAGES ------------------
            if (model.Images != null)
            {
                foreach (var img in model.Images)
                {
                    await _unitOfWork.PropertyImageRepository.AddAsync(new TblPropertyImage
                    {
                        PropertyID = entity.PropertyID,
                        ImagePath = img.ImagePath,
                        UploadedDate = DateTime.Now,
                        IsDeleted = false
                    });
                }
            }

            // ------------------ FEATURES ------------------
            if (model.SelectedFeatures != null)
            {
                foreach (var featureId in model.SelectedFeatures)
                {
                    await _unitOfWork.PropertyFeaturesMappingRepository.AddAsync(
                        new TblPropertyFeaturesMapping
                        {
                            PropertyID = entity.PropertyID,
                            FeatureID = featureId
                        });
                }
            }

            await _unitOfWork.CompleteAsync();
        }

        // ---------------------------------------------------------
        // 4. UPDATE Property
        // ---------------------------------------------------------
        public async Task UpdatePropertyAsync(PropertyViewModel model)
        {
            var entity = await _unitOfWork.PropertyRepository.GetByIdAsync(model.PropertyID);
            if (entity == null) return;

            string savedPropertyCode = entity.PropertyCode;

            entity = ConvertToEntity(model);
            entity.PropertyCode = savedPropertyCode;

            await _unitOfWork.PropertyRepository.UpdateAsync(entity);

            // ------------------ IMAGES ------------------
            var allImages = await _unitOfWork.PropertyImageRepository.ReadAllAsync();
            var oldImages = allImages.Where(i => i.PropertyID == model.PropertyID);

            foreach (var img in oldImages)
            {
                img.IsDeleted = true;
                await _unitOfWork.PropertyImageRepository.UpdateAsync(img);
            }

            if (model.Images != null)
            {
                foreach (var img in model.Images)
                {
                    await _unitOfWork.PropertyImageRepository.AddAsync(
                        new TblPropertyImage
                        {
                            PropertyID = model.PropertyID,
                            ImagePath = img.ImagePath,
                            UploadedDate = DateTime.Now
                        });
                }
            }

            // Get existing feature mappings
            var oldMappings = await _unitOfWork.PropertyFeaturesMappingRepository.ReadAllAsync();
            var oldList = oldMappings.Where(f => f.PropertyID == model.PropertyID);

            // Remove all old mappings
            foreach (var map in oldList)
            {
                await _unitOfWork.PropertyFeaturesMappingRepository
                    .DeletePropertyFeatureMappingAsync(map.PropertyID, map.FeatureID);
            }

            // Add new mappings
            foreach (int featureId in model.SelectedFeatures)
            {
                await _unitOfWork.PropertyFeaturesMappingRepository.AddAsync(
                    new TblPropertyFeaturesMapping
                    {
                        PropertyID = model.PropertyID,
                        FeatureID = featureId
                    });
            }
            await _unitOfWork.CompleteAsync();
        }

        // ---------------------------------------------------------
        // 5. Soft Delete
        // ---------------------------------------------------------
        public async Task DeletePropertyAsync(int id)
        {
            var entity = await _unitOfWork.PropertyRepository.GetByIdAsync(id);
            if (entity == null) return;

            entity.IsDeleted = true;

            await _unitOfWork.PropertyRepository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        // ---------------------------------------------------------
        // 6. Search Helper
        // ---------------------------------------------------------
        public async ValueTask<IEnumerable<TblProperty>> SearchPropertyAsync(Expression<Func<TblProperty, bool>> predicate)
        {
            return await _unitOfWork.PropertyRepository.Search(predicate);
        }

        // ---------------------------------------------------------
        // 7. Features List
        // ---------------------------------------------------------
        public async Task<List<PropertyFeatureViewModel>> GetAllFeaturesAsync()
        {
            var list = await _unitOfWork.PropertyFeatureRepository.ReadAllAsync();

            return list.Select(f => new PropertyFeatureViewModel
            {
                FeatureID = f.FeatureID,
                FeatureName = f.FeatureName
            }).ToList();
        }

        // ---------------------------------------------------------
        // 8. Lookups
        // ---------------------------------------------------------
        public async Task<IEnumerable<LkpPropertyTypeViewModel>> GetAllPropertyTypesAsync()
        {
            var list = await _unitOfWork.PropertyTypeRepository.ReadAllAsync();
            return list.Select(t => new LkpPropertyTypeViewModel
            {
                PropertyTypeID = t.PropertyTypeID,
                TypeName = t.TypeName
            });
        }

        public async Task<IEnumerable<PropertyStatusViewModel>> GetAllStatusesAsync()
        {
            var list = await _unitOfWork.PropertyStatusRepository.ReadAllAsync();
            return list.Select(s => new PropertyStatusViewModel
            {
                StatusID = s.StatusID,
                StatusName = s.StatusName
            });
        }

        public async Task<IEnumerable<DeveloperProfileViewModel>> GetAllDevelopersAsync()
        {
            var list = await _unitOfWork.DeveloperProfileRepository.ReadAllAsync();
            return list.Select(d => new DeveloperProfileViewModel
            {
                DeveloperProfileID = d.DeveloperProfileID,
                DeveloperName = d.DeveloperName
            });
        }

        public async Task<IEnumerable<ZonesViewModel>> GetAllZonesAsync()
        {
            var list = await _unitOfWork.ZoneRepository.ReadAllAsync();
            return list.Select(z => new ZonesViewModel
            {
                ZoneId = z.ZoneID,
                ZoneName = z.ZoneName
            });
        }
        // ---------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------
        public int GetMaxID() =>
            _unitOfWork.PropertyRepository.GetMaxId();

        public async Task<int> GetPropertyCounterAsync() =>
            (await _unitOfWork.PropertyRepository.ReadAllAsync()).Count();

        // ---------------------------------------------------------
        // Mapping Helpers
        // ---------------------------------------------------------
        private PropertyViewModel ConvertToViewModel(TblProperty p)
        {
            return new PropertyViewModel
            {
                PropertyID = p.PropertyID,
                Address = p.Address,
                Area = p.Area,
                Price = p.Price,
                BedsNo = p.BedsNo,
                BathsNo = p.BathsNo,
                FloorsNo = p.FloorsNo,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Description = p.Description,
                AgentId = p.AgentId,
                PropertyTypeID = p.PropertyTypeID,
                DeveloperProfileID = p.DeveloperProfileID,
                ZoneID = p.ZoneID,
                StatusId = p.StatusId,
                PropertyCode = p.PropertyCode,
                ExpectedRentPrice = p.ExpectedRentPrice,
                YearBuilt = p.YearBuilt ?? 0,
                ListingDate = p.ListingDate ?? DateTime.Now,
                IsFurnished = p.IsFurnished,
                IsDeleted = p.IsDeleted,

                DeveloperTitle = p.DeveloperProfile?.DeveloperTitle,
                PropertyTypeName = p.PropertyType?.TypeName,
                StatusName = p.Status?.StatusName,
                ZoneName = p.Zone?.ZoneName,
                AgentName = $"{p.Agent?.FirstName} {p.Agent?.LastName}",

                Images = p.TblPropertyImages
                    ?.Where(i => i.IsDeleted == false)
                    .Select(i => new PropertyImageViewModel
                    {
                        ImageID = i.ImageID,
                        ImagePath = i.ImagePath,
                        UploadedDate = i.UploadedDate,
                        IsDeleted = i.IsDeleted
                    }).ToList() ?? new(),

                SelectedFeatures = p.TblPropertyFeaturesMappings
                    ?.Select(f => f.FeatureID)
                    .ToList() ?? new List<int>()
            };
        }

        private TblProperty ConvertToEntity(PropertyViewModel vm)
        {
            return new TblProperty
            {
                PropertyID = vm.PropertyID,
                Address = vm.Address,
                Area = vm.Area,
                Price = vm.Price,
                BedsNo = vm.BedsNo,
                BathsNo = vm.BathsNo,
                FloorsNo = vm.FloorsNo,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                Description = vm.Description,
                AgentId = vm.AgentId,
                PropertyTypeID = vm.PropertyTypeID,
                DeveloperProfileID = vm.DeveloperProfileID,
                ZoneID = vm.ZoneID,
                StatusId = vm.StatusId,
                ExpectedRentPrice = vm.ExpectedRentPrice,
                IsFurnished = vm.IsFurnished,
                YearBuilt = vm.YearBuilt,
                ListingDate = vm.ListingDate,
                IsDeleted = vm.IsDeleted ?? false
            };
        }
    }
}
