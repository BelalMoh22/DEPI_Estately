using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;

namespace Estately.Services.Implementations
{
    public class ServiceProperty : IServiceProperty
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProperty(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 1. Paged + Search
        public async Task<PropertyListViewModel> GetPropertiesPagedAsync(int page, int pageSize, string? search)
        {
            var properties = await _unitOfWork.PropertyRepository.ReadAllIncluding(
                "DeveloperProfile", "PropertyType", "Status", "Zone"
            );

            var query = properties.AsQueryable().Where(p => p.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(search))
            {
                string s = search.ToLower();
                query = query.Where(p =>
                    (p.Address ?? "").ToLower().Contains(s) ||
                    (p.PropertyCode ?? "").ToLower().Contains(s) ||
                    (p.DeveloperProfile!.DeveloperName ?? "").ToLower().Contains(s)
                );
            }

            int totalCount = query.Count();

            var pagedList = query
                .OrderBy(p => p.PropertyID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PropertyListViewModel
            {
                Properties = pagedList.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = search,
                TotalCount = totalCount
            };
        }

        // 2. Get by id
        public async Task<PropertyViewModel?> GetPropertyByIdAsync(int id)
        {
            var all = await _unitOfWork.PropertyRepository.ReadAllIncluding(
                "DeveloperProfile", "PropertyType", "Status", "Zone"
            );

            var entity = all.FirstOrDefault(p => p.PropertyID == id && p.IsDeleted == false);
            return entity == null ? null : ConvertToViewModel(entity);
        }

        // 3. Create (auto-generate PropertyCode after saving to get ID)
        public async Task CreatePropertyAsync(PropertyViewModel model)
        {
            // Convert -> entity, set defaults
            var entity = ConvertToEntity(model);
            entity.PropertyCode = ""; // placeholder
            entity.IsDeleted = false;
            entity.ListingDate = model.ListingDate == default(DateTime) ? DateTime.Now : model.ListingDate;

            await _unitOfWork.PropertyRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            // generate code using saved id
            entity.PropertyCode = $"PROP-{entity.PropertyID:D3}";

            await _unitOfWork.PropertyRepository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        // 4. Update (do NOT change PropertyCode)
        public async Task UpdatePropertyAsync(PropertyViewModel model)
        {
            var entity = await _unitOfWork.PropertyRepository.GetByIdAsync(model.PropertyID);
            if (entity == null) return;

            entity.Address = model.Address;
            entity.Area = model.Area;
            entity.BedsNo = model.BedsNo;
            entity.BathsNo = model.BathsNo;
            entity.FloorsNo = model.FloorsNo;
            entity.Price = model.Price;
            entity.Description = model.Description;
            entity.Latitude = model.Latitude;
            entity.Longitude = model.Longitude;
            entity.PropertyTypeID = model.PropertyTypeID;
            entity.StatusId = model.StatusId;
            entity.DeveloperProfileID = model.DeveloperProfileID;
            entity.ZoneID = model.ZoneID;
            entity.AgentId = model.AgentId;
            entity.YearBuilt = model.YearBuilt;
            entity.ExpectedRentPrice = model.ExpectedRentPrice;
            entity.IsFurnished = model.IsFurnished;
            entity.ListingDate = model.ListingDate == default(DateTime) ? (entity.ListingDate ?? DateTime.Now) : model.ListingDate;
            // DO NOT set entity.PropertyCode here

            await _unitOfWork.PropertyRepository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        // 5. Delete (soft)
        public async Task DeletePropertyAsync(int id)
        {
            var entity = await _unitOfWork.PropertyRepository.GetByIdAsync(id);
            if (entity == null) return;

            entity.IsDeleted = true;
            await _unitOfWork.PropertyRepository.UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        // 6. Search helper
        public async ValueTask<IEnumerable<TblProperty>> SearchPropertyAsync(Expression<Func<TblProperty, bool>> predicate)
        {
            return await _unitOfWork.PropertyRepository.Search(predicate);
        }

        // 7. Lookups
        public async Task<IEnumerable<LkpPropertyTypeViewModel>> GetAllPropertyTypesAsync()
        {
            var list = await _unitOfWork.PropertyTypeRepository.ReadAllAsync();
            return list.Select(t => new LkpPropertyTypeViewModel { PropertyTypeID = t.PropertyTypeID, TypeName = t.TypeName });
        }

        public async Task<IEnumerable<LkpPropertyStatusViewModel>> GetAllStatusesAsync()
        {
            var list = await _unitOfWork.PropertyStatusRepository.ReadAllAsync();
            return list.Select(s => new LkpPropertyStatusViewModel { StatusID = s.StatusID, StatusName = s.StatusName });
        }

        public async Task<IEnumerable<DeveloperProfileViewModel>> GetAllDevelopersAsync()
        {
            var list = await _unitOfWork.DeveloperProfileRepository.ReadAllAsync();
            return list.Select(d => new DeveloperProfileViewModel { DeveloperProfileID = d.DeveloperProfileID, DeveloperName = d.DeveloperName });
        }

        public async Task<IEnumerable<ZonesViewModel>> GetAllZonesAsync()
        {
            var list = await _unitOfWork.ZoneRepository.ReadAllAsync();
            return list.Select(z => new ZonesViewModel { ZoneId = z.ZoneID, ZoneName = z.ZoneName });
        }

        public async Task<IEnumerable<TblEmployee>> GetAgentsAsync()
        {
            var employees = await _unitOfWork.EmployeeRepository.ReadAllAsync();

            return employees
                .Where(e => e.JobTitleId == 8) // Agents
                .Select(e => new TblEmployee
                {
                    EmployeeID = e.EmployeeID,
                    FirstName = e.FirstName,
                    LastName = e.LastName
                });
        }


        public int GetMaxID() => _unitOfWork.PropertyRepository.GetMaxId();
        public async Task<int> GetPropertyCounterAsync() => await _unitOfWork.PropertyRepository.CounterAsync();

        // Mapping helpers
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
                YearBuilt = p.YearBuilt ?? DateTime.Now.Year,
                ListingDate = p.ListingDate ?? DateTime.Now,
                IsFurnished = p.IsFurnished,
                IsDeleted = p.IsDeleted,

                DeveloperName = p.DeveloperProfile?.DeveloperName,
                PropertyTypeName = p.PropertyType?.TypeName,
                StatusName = p.Status?.StatusName,
                ZoneName = p.Zone?.ZoneName,
                AgentName = p.Agent?.FirstName + " " + p.Agent?.LastName
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
                ListingDate = vm.ListingDate == default(DateTime) ? DateTime.Now : vm.ListingDate,
                IsDeleted = vm.IsDeleted ?? false
            };
        }
    }
}
