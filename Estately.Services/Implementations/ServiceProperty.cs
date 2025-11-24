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

            var query = props.Where(p => p != null && p.IsDeleted == false).AsQueryable();

            // Search term filter (optional)
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(p =>
                    (p.Address ?? "").ToLower().Contains(search) ||
                    (p.PropertyCode ?? "").ToLower().Contains(search) ||
                    (p.DeveloperProfile != null && (p.DeveloperProfile.DeveloperTitle ?? "").ToLower().Contains(search))
                );
            }

            // Ensure page and pageSize are valid
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            // Get total count before pagination
            int total = query.Count();

            // Handle empty results
            if (total == 0)
            {
                return new PropertyListViewModel
                {
                    Properties = new List<PropertyViewModel>(),
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = 0,
                    SearchTerm = search,
                    Features = await GetAllFeaturesAsync()
                };
            }

            // Apply pagination with proper null checks
            var paged = query
                .Where(p => p != null && p.PropertyID > 0) // Additional safety check
                .OrderBy(p => p.PropertyID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PropertyListViewModel
            {
                Properties = paged.Where(p => p != null).Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                SearchTerm = search,
                Features = await GetAllFeaturesAsync()
            };
        }

        // ----------------------------------------------------
        // FILTERED LIST
        // ----------------------------------------------------
        public async Task<PropertyListViewModel> GetPropertiesFilteredAsync(
            int page,
            int pageSize,
            string? search = null,
            string? city = null,
            string? zones = null,
            string? developers = null,
            string? propertyTypes = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            decimal? minArea = null,
            decimal? maxArea = null,
            int? bedrooms = null,
            int? bathrooms = null,
            string? amenities = null)
        {
            var props = await _unitOfWork.PropertyRepository.ReadAllIncluding(
                "DeveloperProfile",
                "PropertyType",
                "Status",
                "Zone",
                "Zone.City",
                "TblPropertyImages",
                "TblPropertyFeaturesMappings",
                "TblPropertyFeaturesMappings.Feature"
            );

            var query = props.Where(p => p != null && p.IsDeleted == false).AsQueryable();

            // Search term filter (optional)
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(p =>
                    (p.Address ?? "").ToLower().Contains(search) ||
                    (p.PropertyCode ?? "").ToLower().Contains(search) ||
                    (p.DeveloperProfile != null && (p.DeveloperProfile.DeveloperTitle ?? "").ToLower().Contains(search)) ||
                    (p.Zone != null && (p.Zone.ZoneName ?? "").ToLower().Contains(search)) ||
                    (p.Zone != null && p.Zone.City != null && (p.Zone.City.CityName ?? "").ToLower().Contains(search))
                );
            }

            // City filter - show all properties in all zones of selected city
            if (!string.IsNullOrWhiteSpace(city) && int.TryParse(city, out var cityId))
            {
                query = query.Where(p => p.Zone != null && p.Zone.City != null && p.Zone.City.CityID == cityId);
            }

            // Zones filter (kept for backward compatibility)
            if (!string.IsNullOrWhiteSpace(zones))
            {
                var zoneIds = zones.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(z => int.TryParse(z.Trim(), out var id) ? id : (int?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id!.Value)
                    .ToList();
                
                if (zoneIds.Any())
                {
                    query = query.Where(p => zoneIds.Contains(p.ZoneID));
                }
            }

            // Developers filter - support both ID and DeveloperName
            if (!string.IsNullOrWhiteSpace(developers))
            {
                var developerParts = developers.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(d => d.Trim())
                    .ToList();
                
                if (developerParts.Any())
                {
                    // Try to parse as IDs first
                    var developerIds = developerParts
                        .Select(d => int.TryParse(d, out var id) ? id : (int?)null)
                        .Where(id => id.HasValue)
                        .Select(id => id!.Value)
                        .ToList();
                    
                    // Also get developers by name
                    var allDevelopers = await _unitOfWork.DeveloperProfileRepository.ReadAllAsync();
                    var developersByName = allDevelopers
                        .Where(d => developerParts.Any(part => 
                            (d.DeveloperName ?? "").Equals(part, StringComparison.OrdinalIgnoreCase) ||
                            (d.DeveloperTitle ?? "").Equals(part, StringComparison.OrdinalIgnoreCase)))
                        .Select(d => d.DeveloperProfileID)
                        .ToList();
                    
                    // Combine IDs
                    var allDeveloperIds = developerIds.Union(developersByName).Distinct().ToList();
                    
                    if (allDeveloperIds.Any())
                    {
                        query = query.Where(p => p.DeveloperProfileID.HasValue && allDeveloperIds.Contains(p.DeveloperProfileID.Value));
                    }
                }
            }

            // Property types filter
            if (!string.IsNullOrWhiteSpace(propertyTypes))
            {
                var typeNames = propertyTypes.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim())
                    .ToList();
                
                if (typeNames.Any())
                {
                    query = query.Where(p => p.PropertyType != null && typeNames.Contains(p.PropertyType.TypeName));
                }
            }

            // Price range filter
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Area range filter
            if (minArea.HasValue)
            {
                query = query.Where(p => p.Area >= minArea.Value);
            }
            if (maxArea.HasValue)
            {
                query = query.Where(p => p.Area <= maxArea.Value);
            }

            // Bedrooms filter
            if (bedrooms.HasValue)
            {
                if (bedrooms.Value >= 5)
                {
                    query = query.Where(p => p.BedsNo >= 5);
                }
                else
                {
                    query = query.Where(p => p.BedsNo == bedrooms.Value);
                }
            }

            // Bathrooms filter
            if (bathrooms.HasValue)
            {
                if (bathrooms.Value >= 5)
                {
                    query = query.Where(p => p.BathsNo >= 5);
                }
                else
                {
                    query = query.Where(p => p.BathsNo == bathrooms.Value);
                }
            }

            // Amenities filter
            if (!string.IsNullOrWhiteSpace(amenities))
            {
                var amenityNames = amenities.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim())
                    .ToList();
                
                if (amenityNames.Any())
                {
                    // Get feature IDs for the amenity names
                    var allFeatures = await _unitOfWork.PropertyFeatureRepository.ReadAllAsync();
                    var featureIds = allFeatures
                        .Where(f => amenityNames.Contains(f.FeatureName, StringComparer.OrdinalIgnoreCase))
                        .Select(f => f.FeatureID)
                        .ToList();
                    
                    if (featureIds.Any())
                    {
                        var propertyIdsWithFeatures = await _unitOfWork.PropertyFeaturesMappingRepository.ReadAllAsync();
                        var matchingPropertyIds = propertyIdsWithFeatures
                            .Where(m => featureIds.Contains(m.FeatureID))
                            .Select(m => m.PropertyID)
                            .Distinct()
                            .ToList();
                        
                        query = query.Where(p => matchingPropertyIds.Contains(p.PropertyID));
                    }
                }
            }

            // Ensure page and pageSize are valid
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            // Get total count before pagination
            int total = query.Count();

            // Handle empty results
            if (total == 0)
            {
                return new PropertyListViewModel
                {
                    Properties = new List<PropertyViewModel>(),
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = 0,
                    SearchTerm = search,
                    Features = await GetAllFeaturesAsync()
                };
            }

            // Apply pagination with proper null checks
            var paged = query
                .Where(p => p != null && p.PropertyID > 0) // Additional safety check
                .OrderBy(p => p.PropertyID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PropertyListViewModel
            {
                Properties = paged.Where(p => p != null).Select(ConvertToViewModel).ToList(),
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
                StatusName = p.Status.StatusName ?? "Available",
                ZoneName = p.Zone?.ZoneName ?? "",
                CityName = p.Zone?.City?.CityName ?? "",
                AgentName = $"{p.Agent?.FirstName} {p.Agent?.LastName}",
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