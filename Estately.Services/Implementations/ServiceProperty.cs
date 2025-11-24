// ---------------------------------------------------------
// SERVICE PROPERTY  (UPDATED)
// ---------------------------------------------------------
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
        // ----------------------------------------------------
        // PAGED LIST
        // ----------------------------------------------------
        public async Task<PropertyListViewModel> GetPropertiesPagedAsync(int page, int pageSize, string? search)
        {
            var props = await _unitOfWork.PropertyRepository.ReadAllIncluding(
                "DeveloperProfile",
                "PropertyType",
                "Status",
                "Zone",
                "Agent",
                "TblPropertyImages",
                "TblPropertyFeaturesMappings"
            );

            var query = props.Where(p => p.IsDeleted == false).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(p =>
                    (p.Address ?? "").ToLower().Contains(search) ||
                    (p.PropertyCode ?? "").ToLower().Contains(search) ||
                    (p.DeveloperProfile!.DeveloperTitle ?? "").ToLower().Contains(search)
                );
            }

            int total = query.Count();

            var paged = query.OrderBy(p => p.PropertyID)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new PropertyListViewModel
            {
                Properties = paged.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                SearchTerm = search,
                Features = await GetAllFeaturesAsync()
            };
        }

        // ----------------------------------------------------
        // GET BY ID
        // ----------------------------------------------------
        public async Task<PropertyViewModel?> GetPropertyByIdAsync(int id)
        {
            var props = await _unitOfWork.PropertyRepository.ReadAllIncluding(
                "DeveloperProfile",
                "PropertyType",
                "Status",
                "Zone",
                "Agent",
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

        // ----------------------------------------------------
        // CREATE
        // ----------------------------------------------------
        public async Task CreatePropertyAsync(PropertyViewModel model)
        {
            var entity = ConvertToEntity(model);
            entity.PropertyCode = ""; // set later after insert

            await _unitOfWork.PropertyRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            // Generate property code => PROP-ZONENAME-ID
            var zone = await _unitOfWork.ZoneRepository.GetByIdAsync(entity.ZoneID);
            string zoneName = zone?.ZoneName?.Replace(" ", "") ?? "Unknown";

            entity.PropertyCode = $"PROP-{zoneName}-{entity.PropertyID:D4}";

            // Save images
            if (model.Images != null)
            {
                foreach (var img in model.Images)
                {
                    await _unitOfWork.PropertyImageRepository.AddAsync(new TblPropertyImage
                    {
                        PropertyID = entity.PropertyID,
                        ImagePath = img.ImagePath,
                        UploadedDate = DateTime.Now,
                    });
                }
            }

            // Save features
            if (model.SelectedFeatures != null)
            {
                foreach (var fid in model.SelectedFeatures)
                {
                    await _unitOfWork.PropertyFeaturesMappingRepository.AddAsync(
                        new TblPropertyFeaturesMapping
                        {
                            PropertyID = entity.PropertyID,
                            FeatureID = fid
                        });
                }
            }

            await _unitOfWork.CompleteAsync();
        }

        // ----------------------------------------------------
        // UPDATE
        // ----------------------------------------------------
        public async Task UpdatePropertyAsync(PropertyViewModel model)
        {
            var entity = await _unitOfWork.PropertyRepository.GetByIdAsync(model.PropertyID);
            if (entity == null) return;

            // EF is tracking this entity, so modifying fields is enough
            entity.Address = model.Address;
            entity.Area = model.Area;
            entity.Price = model.Price;
            entity.BedsNo = model.BedsNo;
            entity.BathsNo = model.BathsNo;
            entity.FloorNo = model.FloorNo;
            entity.Latitude = model.Latitude;
            entity.Longitude = model.Longitude;
            entity.Description = model.Description;
            entity.AgentId = model.AgentId;
            entity.PropertyTypeID = model.PropertyTypeID;
            entity.DeveloperProfileID = model.DeveloperProfileID;
            entity.ZoneID = model.ZoneID;
            entity.StatusId = model.StatusId;
            entity.ExpectedRentPrice = model.ExpectedRentPrice;
            entity.YearBuilt = model.YearBuilt;
            entity.ListingDate = model.ListingDate;

            // ❌ Remove this line:
            // await _unitOfWork.PropertyRepository.UpdateAsync(entity);

            // Save new images
            if (model.Images != null)
            {
                foreach (var img in model.Images)
                {
                    await _unitOfWork.PropertyImageRepository.AddAsync(new TblPropertyImage
                    {
                        PropertyID = model.PropertyID,
                        ImagePath = img.ImagePath,
                        UploadedDate = DateTime.Now
                    });
                }
            }

            // Replace features
            var maps = await _unitOfWork.PropertyFeaturesMappingRepository
                                        .ReadAllAsync();

            var oldMaps = maps.Where(f => f.PropertyID == model.PropertyID).ToList();

            foreach (var m in oldMaps)
            {
                await _unitOfWork.PropertyFeaturesMappingRepository
                    .DeletePropertyFeatureMappingAsync(m.PropertyID, m.FeatureID);
            }

            foreach (var fid in model.SelectedFeatures)
            {
                await _unitOfWork.PropertyFeaturesMappingRepository.AddAsync(
                    new TblPropertyFeaturesMapping
                    {
                        PropertyID = model.PropertyID,
                        FeatureID = fid
                    });
            }

            // Final save
            await _unitOfWork.CompleteAsync();
        }


        // ----------------------------------------------------
        // DELETE
        // ----------------------------------------------------
        public async Task DeletePropertyAsync(int id)
        {
            var entity = await _unitOfWork.PropertyRepository.GetByIdAsync(id);
            if (entity == null) return;
            // Only allow delete when status is "Unavailable"
            var statuses = await _unitOfWork.PropertyStatusRepository.ReadAllAsync();
            var unavailableStatus = statuses.FirstOrDefault(s => s.StatusName == "Unavailable");

            if (unavailableStatus == null || entity.StatusId != unavailableStatus.StatusID)
            {
                // Do not delete if status is not "Unavailable"
                return;
            }

            entity.IsDeleted = true;

            await _unitOfWork.PropertyRepository.UpdateAsync(entity);

            // Also remove associated images from TblPropertyImage
            var allImages = await _unitOfWork.PropertyImageRepository.ReadAllAsync();
            var imagesForProperty = allImages.Where(i => i.PropertyID == id).ToList();

            foreach (var img in imagesForProperty)
            {
                await _unitOfWork.PropertyImageRepository.DeleteAsync(img.ImageID);
            }

            await _unitOfWork.CompleteAsync();
        }

        // ----------------------------------------------------
        // LOOKUPS
        // ----------------------------------------------------
        public async Task<List<PropertyFeatureViewModel>> GetAllFeaturesAsync()
        {
            var list = await _unitOfWork.PropertyFeatureRepository.ReadAllAsync();
            return list.Select(f => new PropertyFeatureViewModel
            {
                FeatureID = f.FeatureID,
                FeatureName = f.FeatureName
            }).ToList();
        }

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
                DeveloperTitle = d.DeveloperTitle
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

        // ----------------------------------------------------
        // MAPPING HELPERS
        // ----------------------------------------------------
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
                FloorNo = p.FloorNo,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Description = p.Description,
                AgentId = p.AgentId,
                PropertyTypeID = p.PropertyTypeID,
                DeveloperProfileID = p.DeveloperProfileID ?? null,
                ZoneID = p.ZoneID,
                StatusId = p.StatusId ?? 1,
                PropertyCode = p.PropertyCode,
                ExpectedRentPrice = p.ExpectedRentPrice,
                YearBuilt = p.YearBuilt ?? 2025,
                ListingDate = p.ListingDate ?? DateTime.Now,
                IsDeleted = p.IsDeleted,

                DeveloperTitle = p.DeveloperProfile?.DeveloperTitle,
                PropertyTypeName = p.PropertyType?.TypeName,
                StatusName = p.Status?.StatusName ?? "Available",
                ZoneName = p.Zone?.ZoneName,
                AgentName = p.Agent != null ? $"{p.Agent.FirstName} {p.Agent.LastName}" : null,

                Images = p.TblPropertyImages?
                    .Select(i => new PropertyImageViewModel
                    {
                        ImageID = i.ImageID,
                        ImagePath = i.ImagePath,
                        UploadedDate = i.UploadedDate,
                    }).ToList() ?? new(),

                SelectedFeatures = p.TblPropertyFeaturesMappings?
                    .Select(f => f.FeatureID)
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
                FloorNo = vm.FloorNo,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                Description = vm.Description,
                AgentId = vm.AgentId,
                PropertyTypeID = vm.PropertyTypeID,
                DeveloperProfileID = vm.DeveloperProfileID,
                ZoneID = vm.ZoneID,
                StatusId = vm.StatusId,
                ExpectedRentPrice = vm.ExpectedRentPrice,
                YearBuilt = vm.YearBuilt,
                ListingDate = vm.ListingDate,
                IsDeleted = vm.IsDeleted ?? false
            };
        }
        // ----------------------------------------------------
        // REQUIRED INTERFACE METHODS (ADDED)
        // ----------------------------------------------------

        public async ValueTask<IEnumerable<TblProperty>> SearchPropertyAsync(Expression<Func<TblProperty, bool>> predicate)
        {
            var list = await _unitOfWork.PropertyRepository.ReadAllAsync();
            return list.Where(predicate.Compile());
        }

        public async Task<int> GetMaxIDAsync()
        {
            var list = await _unitOfWork.PropertyRepository.ReadAllAsync();
            return list.Any() ? list.Max(p => p.PropertyID) : 0;
        }

        public async Task<int> GetPropertyCounterAsync()
        {
            var list = await _unitOfWork.PropertyRepository.ReadAllAsync();
            return list.Count(p => p.IsDeleted == false);
        }
    }
}