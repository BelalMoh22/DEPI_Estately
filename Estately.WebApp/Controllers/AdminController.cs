using Estately.Core.Entities;
using Estately.Core.Interfaces;
using Estately.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Estately.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Dashboard

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {

            var stats = new
            {
                TotalUsers = (await _unitOfWork.UserRepository.ReadAllAsync()).Count(),
                TotalProperties = (await _unitOfWork.PropertyRepository.ReadAllAsync()).Count(),
                TotalAppointments = (await _unitOfWork.AppointmentRepository.ReadAllAsync()).Count(),
                TotalEmployees = (await _unitOfWork.EmployeeRepository.ReadAllAsync()).Count(),
                TotalClients = (await _unitOfWork.ClientProfileRepository.ReadAllAsync()).Count(),
                TotalBranches = (await _unitOfWork.BranchRepository.ReadAllAsync()).Count()
            };

            ViewBag.Stats = stats;
            return View();
        }

        #endregion

        #region Users CRUD

        [HttpGet]
        public async Task<IActionResult> Users(int page = 1, int pageSize = 10, string? searchTerm = null)
        {

            var allUsers = await _unitOfWork.UserRepository.ReadAllIncluding("UserType");
            var query = allUsers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.Email.Contains(searchTerm) || u.Username.Contains(searchTerm));
            }

            var totalCount = query.Count();
            var users = query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new UserListViewModel
            {
                Users = users.Select(u => new UserViewModel
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
                }).ToList(),
                UserTypes = (await _unitOfWork.UserTypeRepository.ReadAllAsync())
                    .Select(ut => new LkpUserTypeViewModel
                    {
                        UserTypeID = ut.UserTypeID,
                        UserTypeName = ut.UserTypeName,
                        Description = ut.Description
                    }).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                TotalCount = totalCount
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.UserTypes = new SelectList(await _unitOfWork.UserTypeRepository.ReadAllAsync(), "UserTypeID", "UserTypeName");
            return View(new UserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new TblUser
                {
                    Email = model.Email,
                    Username = model.Username,
                    PasswordHash = model.PasswordHash ?? "DefaultPassword123",
                    UserTypeID = model.UserTypeID ?? 1,
                    IsEmployee = model.IsEmployee ?? false,
                    IsClient = model.IsClient ?? true,
                    IsDeveloper = model.IsDeveloper ?? false,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                _unitOfWork.UserRepository.AddAsync(user);
                _unitOfWork.CompleteAsync();

                return RedirectToAction("Users");
            }

            ViewBag.UserTypes = new SelectList(await _unitOfWork.UserTypeRepository.ReadAllAsync(), "UserTypeID", "UserTypeName");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            var model = new UserViewModel
            {
                UserID = user.UserID,
                UserTypeID = user.UserTypeID,
                Email = user.Email,
                Username = user.Username,
                IsEmployee = user.IsEmployee,
                IsClient = user.IsClient,
                IsDeveloper = user.IsDeveloper,
                CreatedAt = user.CreatedAt,
                IsDeleted = user.IsDeleted
            };

            ViewBag.UserTypes = new SelectList(await _unitOfWork.UserTypeRepository.ReadAllAsync(), "UserTypeID", "UserTypeName", user.UserTypeID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(model.UserID);
                if (user == null) return NotFound();

                user.Email = model.Email;
                user.Username = model.Username;
                user.UserTypeID = model.UserTypeID;
                user.IsEmployee = model.IsEmployee;
                user.IsClient = model.IsClient;
                user.IsDeveloper = model.IsDeveloper;
                user.IsDeleted = model.IsDeleted;

                if (!string.IsNullOrEmpty(model.PasswordHash))
                {
                    user.PasswordHash = model.PasswordHash;
                }

                _unitOfWork.UserRepository.UpdateAsync(user);
                _unitOfWork.CompleteAsync();

                return RedirectToAction("Users");
            }

            ViewBag.UserTypes = new SelectList(await _unitOfWork.UserTypeRepository.ReadAllAsync(), "UserTypeID", "UserTypeName", model.UserTypeID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                _unitOfWork.UserRepository.UpdateAsync(user);
                _unitOfWork.CompleteAsync();
            }
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user != null)
            {
                user.IsDeleted = !user.IsDeleted;
                _unitOfWork.UserRepository.UpdateAsync(user);
                _unitOfWork.CompleteAsync();
            }
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserRole(int userId, int userTypeId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.UserTypeID = userTypeId;
                _unitOfWork.UserRepository.UpdateAsync(user);
                _unitOfWork.CompleteAsync();
            }
            return RedirectToAction("Users");
        }

        #endregion

        #region Properties CRUD

        [HttpGet]
        public async Task<IActionResult> Properties(int page = 1, int pageSize = 10, string? searchTerm = null)
        {

            var allProperties = await _unitOfWork.PropertyRepository.ReadAllIncluding("PropertyType", "Status", "Zone", "DeveloperProfile", "Agent");
            var query = allProperties.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.PropertyCode.Contains(searchTerm) || p.Address.Contains(searchTerm));
            }

            var totalCount = query.Count();
            var properties = query
                .OrderByDescending(p => p.ListingDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PropertyListViewModel
            {
                Properties = properties.Select(p => new PropertyViewModel
                {
                    PropertyID = p.PropertyID,
                    DeveloperProfileID = p.DeveloperProfileID,
                    PropertyTypeID = p.PropertyTypeID,
                    StatusId = p.StatusId,
                    ZoneID = p.ZoneID,
                    Title = p.PropertyCode, // Using PropertyCode as Title since entity doesn't have Title
                    Address = p.Address,
                    Price = p.Price,
                    Description = p.Description,
                    Area = p.Area,
                    ListingDate = p.ListingDate ?? DateTime.Now,
                    ExpectedRentPrice = p.ExpectedRentPrice,
                    IsDeleted = p.IsDeleted,
                    AgentId = p.AgentId,
                    YearBuilt = p.YearBuilt,
                    FloorsNo = p.FloorsNo,
                    BedsNo = p.BedsNo,
                    BathsNo = p.BathsNo,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    IsFurnished = p.IsFurnished,
                    PropertyCode = p.PropertyCode,
                    DeveloperName = p.DeveloperProfile?.DeveloperName,
                    PropertyTypeName = p.PropertyType?.TypeName,
                    StatusName = p.Status?.StatusName,
                    ZoneName = p.Zone?.ZoneName,
                    AgentName = p.Agent != null ? $"{p.Agent.FirstName} {p.Agent.LastName}" : null
                }).ToList(),
                PropertyTypes = (await _unitOfWork.PropertyTypeRepository.ReadAllAsync())
                    .Select(pt => new LkpPropertyTypeViewModel { PropertyTypeID = pt.PropertyTypeID, TypeName = pt.TypeName }).ToList(),
                PropertyStatuses = (await _unitOfWork.PropertyStatusRepository.ReadAllAsync())
                    .Select(ps => new LkpPropertyStatusViewModel { StatusID = ps.StatusID, StatusName = ps.StatusName, Description = ps.Description }).ToList(),
                DeveloperProfiles = (await _unitOfWork.DeveloperProfileRepository.ReadAllIncluding("User"))
                    .Select(dp => new DeveloperProfileViewModel { DeveloperProfileID = dp.DeveloperProfileID, DeveloperName = dp.DeveloperName }).ToList(),
                Zones = (await _unitOfWork.ZoneRepository.ReadAllIncluding("City"))
                    .Select(z => new ZoneViewModel { ZoneID = z.ZoneID, ZoneName = z.ZoneName, CityName = z.City?.CityName }).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                TotalCount = totalCount
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProperty()
        {
            ViewBag.PropertyTypes = new SelectList(await _unitOfWork.PropertyTypeRepository.ReadAllAsync(), "PropertyTypeID", "TypeName");
            ViewBag.PropertyStatuses = new SelectList(await _unitOfWork.PropertyStatusRepository.ReadAllAsync(), "StatusID", "StatusName");
            ViewBag.DeveloperProfiles = new SelectList(await _unitOfWork.DeveloperProfileRepository.ReadAllAsync(), "DeveloperProfileID", "DeveloperName");
            ViewBag.Zones = new SelectList(await _unitOfWork.ZoneRepository.ReadAllAsync(), "ZoneID", "ZoneName");
            var agents = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            ViewBag.Agents = new SelectList(agents.Select(e => new { e.EmployeeID, FullName = $"{e.FirstName} {e.LastName}" }), "EmployeeID", "FullName");
            return View(new PropertyViewModel { ListingDate = DateTime.Now });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProperty(PropertyViewModel model)
        {

            if (ModelState.IsValid)
            {
                var property = new TblProperty
                {
                    DeveloperProfileID = model.DeveloperProfileID,
                    PropertyTypeID = model.PropertyTypeID,
                    StatusId = model.StatusId,
                    ZoneID = model.ZoneID,
                    Address = model.Address,
                    Price = model.Price,
                    Description = model.Description,
                    Area = model.Area,
                    ListingDate = model.ListingDate,
                    ExpectedRentPrice = model.ExpectedRentPrice,
                    IsDeleted = false,
                    AgentId = model.AgentId,
                    YearBuilt = model.YearBuilt,
                    FloorsNo = model.FloorsNo,
                    BedsNo = model.BedsNo,
                    BathsNo = model.BathsNo,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    IsFurnished = model.IsFurnished,
                    PropertyCode = model.PropertyCode
                };

                _unitOfWork.PropertyRepository.AddAsync(property);
                _unitOfWork.CompleteAsync();

                return RedirectToAction("Properties");
            }

            ViewBag.PropertyTypes = new SelectList(await _unitOfWork.PropertyTypeRepository.ReadAllAsync(), "PropertyTypeID", "TypeName");
            ViewBag.PropertyStatuses = new SelectList(await _unitOfWork.PropertyStatusRepository.ReadAllAsync(), "StatusID", "StatusName");
            ViewBag.DeveloperProfiles = new SelectList(await _unitOfWork.DeveloperProfileRepository.ReadAllAsync(), "DeveloperProfileID", "DeveloperName");
            ViewBag.Zones = new SelectList(await _unitOfWork.ZoneRepository.ReadAllAsync(), "ZoneID", "ZoneName");
            var agents = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            ViewBag.Agents = new SelectList(agents.Select(e => new { e.EmployeeID, FullName = $"{e.FirstName} {e.LastName}" }), "EmployeeID", "FullName");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProperty(int id)
        {
            var property = await _unitOfWork.PropertyRepository.GetByIdAsync(id);
            if (property == null) return NotFound();

            var model = new PropertyViewModel
            {
                PropertyID = property.PropertyID,
                DeveloperProfileID = property.DeveloperProfileID,
                PropertyTypeID = property.PropertyTypeID,
                StatusId = property.StatusId,
                ZoneID = property.ZoneID,
                Title = property.PropertyCode, // Using PropertyCode as Title
                Address = property.Address,
                Price = property.Price,
                Description = property.Description,
                Area = property.Area,
                ListingDate = property.ListingDate ?? DateTime.Now,
                ExpectedRentPrice = property.ExpectedRentPrice,
                IsDeleted = property.IsDeleted,
                AgentId = property.AgentId,
                YearBuilt = property.YearBuilt,
                FloorsNo = property.FloorsNo,
                BedsNo = property.BedsNo,
                BathsNo = property.BathsNo,
                Latitude = property.Latitude,
                Longitude = property.Longitude,
                IsFurnished = property.IsFurnished,
                PropertyCode = property.PropertyCode
            };

            ViewBag.PropertyTypes = new SelectList(await _unitOfWork.PropertyTypeRepository.ReadAllAsync(), "PropertyTypeID", "TypeName", property.PropertyTypeID);
            ViewBag.PropertyStatuses = new SelectList(await _unitOfWork.PropertyStatusRepository.ReadAllAsync(), "StatusID", "StatusName", property.StatusId);
            ViewBag.DeveloperProfiles = new SelectList(await _unitOfWork.DeveloperProfileRepository.ReadAllAsync(), "DeveloperProfileID", "DeveloperName", property.DeveloperProfileID);
            ViewBag.Zones = new SelectList(await _unitOfWork.ZoneRepository.ReadAllAsync(), "ZoneID", "ZoneName", property.ZoneID);
            ViewBag.Agents = new SelectList(await _unitOfWork.EmployeeRepository.ReadAllAsync(), "EmployeeID", "FirstName", property.AgentId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProperty(PropertyViewModel model)
        {

            if (ModelState.IsValid)
            {
                var property = await _unitOfWork.PropertyRepository.GetByIdAsync(model.PropertyID);
                if (property == null) return NotFound();

                property.DeveloperProfileID = model.DeveloperProfileID;
                property.PropertyTypeID = model.PropertyTypeID;
                property.StatusId = model.StatusId;
                property.ZoneID = model.ZoneID;
                property.Address = model.Address;
                property.Price = model.Price;
                property.Description = model.Description;
                property.Area = model.Area;
                property.ListingDate = model.ListingDate;
                property.ExpectedRentPrice = model.ExpectedRentPrice;
                property.IsDeleted = model.IsDeleted;
                property.AgentId = model.AgentId;
                property.YearBuilt = model.YearBuilt;
                property.FloorsNo = model.FloorsNo;
                property.BedsNo = model.BedsNo;
                property.BathsNo = model.BathsNo;
                property.Latitude = model.Latitude;
                property.Longitude = model.Longitude;
                property.IsFurnished = model.IsFurnished;
                property.PropertyCode = model.PropertyCode;

                _unitOfWork.PropertyRepository.UpdateAsync(property);
                _unitOfWork.CompleteAsync();

                return RedirectToAction("Properties");
            }

            ViewBag.PropertyTypes = new SelectList(await _unitOfWork.PropertyTypeRepository.ReadAllAsync(), "PropertyTypeID", "TypeName", model.PropertyTypeID);
            ViewBag.PropertyStatuses = new SelectList(await _unitOfWork.PropertyStatusRepository.ReadAllAsync(), "StatusID", "StatusName", model.StatusId);
            ViewBag.DeveloperProfiles = new SelectList(await _unitOfWork.DeveloperProfileRepository.ReadAllAsync(), "DeveloperProfileID", "DeveloperName", model.DeveloperProfileID);
            ViewBag.Zones = new SelectList(await _unitOfWork.ZoneRepository.ReadAllAsync(), "ZoneID", "ZoneName", model.ZoneID);
            ViewBag.Agents = new SelectList(await _unitOfWork.EmployeeRepository.ReadAllAsync(), "EmployeeID", "FirstName", model.AgentId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _unitOfWork.PropertyRepository.GetByIdAsync(id);
            if (property != null)
            {
                property.IsDeleted = true;
                _unitOfWork.PropertyRepository.UpdateAsync(property);
                _unitOfWork.CompleteAsync();
            }
            return RedirectToAction("Properties");
        }

        #endregion

        #region Generic CRUD Helper Methods

        // Generic methods for simpler entities (Cities, Zones, Branches, Departments, etc.)
        // These follow the same pattern but are more concise

        #endregion

        #region Cities CRUD

        [HttpGet]
        public async Task<IActionResult> Cities(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var allCities = await _unitOfWork.CityRepository.ReadAllAsync();
            var query = allCities.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(c => c.CityName.Contains(searchTerm));

            var totalCount = query.Count();
            var cities = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Cities = cities.Select(c => new CityViewModel { CityID = c.CityID, CityName = c.CityName }).ToList();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return View();
        }

        [HttpGet]
        public IActionResult CreateCity()
        {
            return View(new CityViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CityRepository.AddAsync(new TblCity { CityName = model.CityName });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("Cities");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditCity(int id)
        {
            var city = await _unitOfWork.CityRepository.GetByIdAsync(id);
            if (city == null) return NotFound();
            var model = new CityViewModel { CityID = city.CityID, CityName = city.CityName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCity(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var city = await _unitOfWork.CityRepository.GetByIdAsync(model.CityID);
                if (city != null)
                {
                    city.CityName = model.CityName;
                    _unitOfWork.CityRepository.UpdateAsync(city);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("Cities");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCity(int id)
        {
            _unitOfWork.CityRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("Cities");
        }

        #endregion

        #region Zones CRUD

        [HttpGet]
        public async Task<IActionResult> Zones(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var allZones = await _unitOfWork.ZoneRepository.ReadAllIncluding("City");
            var query = allZones.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(z => z.ZoneName.Contains(searchTerm) || (z.City != null && z.City.CityName.Contains(searchTerm)));

            var totalCount = query.Count();
            var zones = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Zones = zones.Select(z => new ZoneViewModel
            {
                ZoneID = z.ZoneID,
                ZoneName = z.ZoneName,
                CityID = z.CityID,
                CityName = z.City?.CityName
            }).ToList();
            ViewBag.Cities = new SelectList(await _unitOfWork.CityRepository.ReadAllAsync(), "CityID", "CityName");
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateZone()
        {
            ViewBag.Cities = new SelectList(await _unitOfWork.CityRepository.ReadAllAsync(), "CityID", "CityName");
            return View(new ZoneViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateZone(ZoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ZoneRepository.AddAsync(new TblZone { ZoneName = model.ZoneName, CityID = model.CityID });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("Zones");
            }
            ViewBag.Cities = new SelectList(await _unitOfWork.CityRepository.ReadAllAsync(), "CityID", "CityName");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditZone(int id)
        {
            var zone = await _unitOfWork.ZoneRepository.GetByIdAsync(id);
            if (zone == null) return NotFound();
            var model = new ZoneViewModel { ZoneID = zone.ZoneID, ZoneName = zone.ZoneName, CityID = zone.CityID };
            ViewBag.Cities = new SelectList(await _unitOfWork.CityRepository.ReadAllAsync(), "CityID", "CityName", zone.CityID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditZone(ZoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var zone = await _unitOfWork.ZoneRepository.GetByIdAsync(model.ZoneID);
                if (zone != null)
                {
                    zone.ZoneName = model.ZoneName;
                    zone.CityID = model.CityID;
                    _unitOfWork.ZoneRepository.UpdateAsync(zone);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("Zones");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteZone(int id)
        {
            _unitOfWork.ZoneRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("Zones");
        }

        #endregion

        #region Branches CRUD

        [HttpGet]
        public async Task<IActionResult> Branches(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var allBranches = await _unitOfWork.BranchRepository.ReadAllAsync();
            var query = allBranches.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(b => b.BranchName.Contains(searchTerm) || b.Address.Contains(searchTerm));

            var totalCount = query.Count();
            var branches = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Branches = branches.Select(b => new BranchViewModel
            {
                BranchID = b.BranchID,
                BranchName = b.BranchName,
                ManagerName = b.ManagerName,
                Address = b.Address,
                Phone = b.Phone,
                IsDeleted = b.IsDeleted
            }).ToList();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return View();
        }

        [HttpGet]
        public IActionResult CreateBranch()
        {
            return View(new BranchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch(BranchViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.BranchRepository.AddAsync(new TblBranch
                {
                    BranchName = model.BranchName,
                    ManagerName = model.ManagerName,
                    Address = model.Address,
                    Phone = model.Phone,
                    IsDeleted = false
                });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("Branches");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditBranch(int id)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);
            if (branch == null) return NotFound();
            var model = new BranchViewModel
            {
                BranchID = branch.BranchID,
                BranchName = branch.BranchName,
                ManagerName = branch.ManagerName,
                Address = branch.Address,
                Phone = branch.Phone,
                IsDeleted = branch.IsDeleted
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBranch(BranchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var branch = await _unitOfWork.BranchRepository.GetByIdAsync(model.BranchID);
                if (branch != null)
                {
                    branch.BranchName = model.BranchName;
                    branch.ManagerName = model.ManagerName;
                    branch.Address = model.Address;
                    branch.Phone = model.Phone;
                    branch.IsDeleted = model.IsDeleted;
                    _unitOfWork.BranchRepository.UpdateAsync(branch);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("Branches");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);
            if (branch != null)
            {
                branch.IsDeleted = true;
                _unitOfWork.BranchRepository.UpdateAsync(branch);
                _unitOfWork.CompleteAsync();
            }
            return RedirectToAction("Branches");
        }

        #endregion

        #region Departments CRUD

        [HttpGet]
        public async Task<IActionResult> Departments(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var allDepartments = await _unitOfWork.DepartmentRepository.ReadAllAsync();
            var query = allDepartments.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(d => d.DepartmentName.Contains(searchTerm));

            var totalCount = query.Count();
            var departments = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Departments = departments.Select(d => new DepartmentViewModel
            {
                DepartmentID = d.DepartmentID,
                DepartmentName = d.DepartmentName,
                ManagerName = d.ManagerName,
                Email = d.Email
            }).ToList();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return View();
        }

        [HttpGet]
        public IActionResult CreateDepartment()
        {
            return View(new DepartmentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.DepartmentRepository.AddAsync(new TblDepartment
                {
                    DepartmentName = model.DepartmentName,
                    ManagerName = model.ManagerName,
                    Email = model.Email
                });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("Departments");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditDepartment(int id)
        {
            var dept = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (dept == null) return NotFound();
            var model = new DepartmentViewModel
            {
                DepartmentID = dept.DepartmentID,
                DepartmentName = dept.DepartmentName,
                ManagerName = dept.ManagerName,
                Email = dept.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDepartment(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dept = await _unitOfWork.DepartmentRepository.GetByIdAsync(model.DepartmentID);
                if (dept != null)
                {
                    dept.DepartmentName = model.DepartmentName;
                    dept.ManagerName = model.ManagerName;
                    dept.Email = model.Email;
                    _unitOfWork.DepartmentRepository.UpdateAsync(dept);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("Departments");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            _unitOfWork.DepartmentRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("Departments");
        }

        #endregion

        #region Appointments CRUD

        [HttpGet]
        public async Task<IActionResult> Appointments(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var allAppointments = await _unitOfWork.AppointmentRepository.ReadAllIncluding("Property", "Status", "EmployeeClient.Employee", "EmployeeClient.ClientProfile");
            var query = allAppointments.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(a => (a.Property != null && a.Property.Address.Contains(searchTerm)) || (a.Notes != null && a.Notes.Contains(searchTerm)));

            var totalCount = query.Count();
            var appointments = query.OrderByDescending(a => a.AppointmentDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Appointments = appointments.Select(a => new AppointmentViewModel
            {
                AppointmentID = a.AppointmentID,
                StatusID = a.StatusID,
                PropertyID = a.PropertyID,
                EmployeeClientID = a.EmployeeClientID,
                AppointmentDate = a.AppointmentDate,
                Notes = a.Notes,
                StatusName = a.Status?.StatusName,
                PropertyTitle = a.Property?.Address,
                ClientName = a.EmployeeClient?.ClientProfile?.FirstName + " " + a.EmployeeClient?.ClientProfile?.LastName,
                EmployeeName = a.EmployeeClient?.Employee?.FirstName + " " + a.EmployeeClient?.Employee?.LastName
            }).ToList();
            ViewBag.Statuses = new SelectList(await _unitOfWork.AppointmentStatusRepository.ReadAllAsync(), "StatusId", "StatusName");
            ViewBag.Properties = new SelectList(await _unitOfWork.PropertyRepository.ReadAllAsync(), "PropertyID", "Title");
            ViewBag.EmployeeClients = new SelectList(await _unitOfWork.EmployeeClientRepository.ReadAllIncluding("Employee", "ClientProfile"), "EmployeeClientID", "EmployeeClientID");
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AppointmentRepository.AddAsync(new TblAppointment
                {
                    StatusID = model.StatusID,
                    PropertyID = model.PropertyID,
                    EmployeeClientID = model.EmployeeClientID,
                    AppointmentDate = model.AppointmentDate,
                    Notes = model.Notes
                });
                _unitOfWork.CompleteAsync();
            }
            return RedirectToAction("Appointments");
        }

        [HttpPost]
        public async Task<IActionResult> EditAppointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(model.AppointmentID);
                if (appointment != null)
                {
                    appointment.StatusID = model.StatusID;
                    appointment.PropertyID = model.PropertyID;
                    appointment.EmployeeClientID = model.EmployeeClientID;
                    appointment.AppointmentDate = model.AppointmentDate;
                    appointment.Notes = model.Notes;
                    _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("Appointments");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            _unitOfWork.AppointmentRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("Appointments");
        }

        #endregion

        #region Employees CRUD

        [HttpGet]
        public async Task<IActionResult> Employees(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var allEmployees = await _unitOfWork.EmployeeRepository.ReadAllIncluding("BranchDepartment", "JobTitle", "ReportsToNavigation", "User");
            var query = allEmployees.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(e => e.FirstName.Contains(searchTerm) || e.LastName.Contains(searchTerm) || e.Phone.Contains(searchTerm));

            var totalCount = query.Count();
            var employees = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Employees = employees.Select(e => new EmployeeViewModel
            {
                EmployeeID = e.EmployeeID,
                UserID = e.UserID,
                BranchDepartmentId = e.BranchDepartmentId,
                JobTitleId = e.JobTitleId,
                ReportsTo = e.ReportsTo,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Gender = e.Gender,
                Age = e.Age,
                Phone = e.Phone,
                Nationalid = e.Nationalid,
                ProfilePhoto = e.ProfilePhoto,
                Salary = e.Salary,
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                BranchDepartmentName = e.BranchDepartment != null ? $"{e.BranchDepartment.Branch?.BranchName} - {e.BranchDepartment.Department?.DepartmentName}" : null,
                JobTitleName = e.JobTitle?.JobTitleName,
                ReportsToName = e.ReportsToNavigation != null ? $"{e.ReportsToNavigation.FirstName} {e.ReportsToNavigation.LastName}" : null,
                Username = e.User?.Username
            }).ToList();
            ViewBag.BranchDepartments = new SelectList(await _unitOfWork.BranchDepartmentRepository.ReadAllIncluding("Branch", "Department"), "BranchDepartmentID", "BranchDepartmentID");
            ViewBag.JobTitles = new SelectList(await _unitOfWork.JobTitleRepository.ReadAllAsync(), "JobTitleId", "JobTitleName");
            var reportsToEmployees = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            ViewBag.ReportsToEmployees = new SelectList(reportsToEmployees.Select(e => new { e.EmployeeID, FullName = $"{e.FirstName} {e.LastName}" }), "EmployeeID", "FullName");
            ViewBag.Users = new SelectList(await _unitOfWork.UserRepository.ReadAllAsync(), "UserID", "Username");
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            ViewBag.BranchDepartments = new SelectList(await _unitOfWork.BranchDepartmentRepository.ReadAllIncluding("Branch", "Department"), "BranchDepartmentID", "BranchDepartmentID");
            ViewBag.JobTitles = new SelectList(await _unitOfWork.JobTitleRepository.ReadAllAsync(), "JobTitleId", "JobTitleName");
            var reportsToEmployees = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            ViewBag.ReportsToEmployees = new SelectList(reportsToEmployees.Select(e => new { e.EmployeeID, FullName = $"{e.FirstName} {e.LastName}" }), "EmployeeID", "FullName");
            ViewBag.Users = new SelectList(await _unitOfWork.UserRepository.ReadAllAsync(), "UserID", "Username");
            return View(new EmployeeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.EmployeeRepository.AddAsync(new TblEmployee
                {
                    UserID = model.UserID,
                    BranchDepartmentId = model.BranchDepartmentId,
                    JobTitleId = model.JobTitleId,
                    ReportsTo = model.ReportsTo,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    Age = model.Age,
                    Phone = model.Phone,
                    Nationalid = model.Nationalid,
                    ProfilePhoto = model.ProfilePhoto,
                    Salary = model.Salary,
                    HireDate = model.HireDate ?? DateTime.Now,
                    IsActive = true
                });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("Employees");
            }
            ViewBag.BranchDepartments = new SelectList(await _unitOfWork.BranchDepartmentRepository.ReadAllIncluding("Branch", "Department"), "BranchDepartmentID", "BranchDepartmentID");
            ViewBag.JobTitles = new SelectList(await _unitOfWork.JobTitleRepository.ReadAllAsync(), "JobTitleId", "JobTitleName");
            var reportsToEmployees = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            ViewBag.ReportsToEmployees = new SelectList(reportsToEmployees.Select(e => new { e.EmployeeID, FullName = $"{e.FirstName} {e.LastName}" }), "EmployeeID", "FullName");
            ViewBag.Users = new SelectList(await _unitOfWork.UserRepository.ReadAllAsync(), "UserID", "Username");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            var model = new EmployeeViewModel
            {
                EmployeeID = employee.EmployeeID,
                UserID = employee.UserID,
                BranchDepartmentId = employee.BranchDepartmentId,
                JobTitleId = employee.JobTitleId,
                ReportsTo = employee.ReportsTo,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Age = employee.Age,
                Phone = employee.Phone,
                Nationalid = employee.Nationalid,
                ProfilePhoto = employee.ProfilePhoto,
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive
            };
            ViewBag.BranchDepartments = new SelectList(await _unitOfWork.BranchDepartmentRepository.ReadAllIncluding("Branch", "Department"), "BranchDepartmentID", "BranchDepartmentID", employee.BranchDepartmentId);
            ViewBag.JobTitles = new SelectList(await _unitOfWork.JobTitleRepository.ReadAllAsync(), "JobTitleId", "JobTitleName", employee.JobTitleId);
            ViewBag.ReportsToEmployees = new SelectList(await _unitOfWork.EmployeeRepository.ReadAllAsync(), "EmployeeID", "FirstName", employee.ReportsTo);
            ViewBag.Users = new SelectList(await _unitOfWork.UserRepository.ReadAllAsync(), "UserID", "Username", employee.UserID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(model.EmployeeID);
                if (employee != null)
                {
                    employee.UserID = model.UserID;
                    employee.BranchDepartmentId = model.BranchDepartmentId;
                    employee.JobTitleId = model.JobTitleId;
                    employee.ReportsTo = model.ReportsTo;
                    employee.FirstName = model.FirstName;
                    employee.LastName = model.LastName;
                    employee.Gender = model.Gender;
                    employee.Age = model.Age;
                    employee.Phone = model.Phone;
                    employee.Nationalid = model.Nationalid;
                    employee.ProfilePhoto = model.ProfilePhoto;
                    employee.Salary = model.Salary;
                    employee.HireDate = model.HireDate;
                    employee.IsActive = model.IsActive;
                    _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("Employees");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee != null)
            {
                employee.IsActive = false;
                _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                _unitOfWork.CompleteAsync();
            }
            return RedirectToAction("Employees");
        }

        #endregion

        #region Lookup Tables CRUD (Property Types, Statuses, etc.)

        [HttpGet]
        public async Task<IActionResult> PropertyTypes()
        {
            ViewBag.PropertyTypes = (await _unitOfWork.PropertyTypeRepository.ReadAllAsync())
                .Select(pt => new LkpPropertyTypeViewModel { PropertyTypeID = pt.PropertyTypeID, TypeName = pt.TypeName }).ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CreatePropertyType()
        {
            return View(new LkpPropertyTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePropertyType(LkpPropertyTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PropertyTypeRepository.AddAsync(new LkpPropertyType { TypeName = model.TypeName });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("PropertyTypes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPropertyType(int id)
        {
            var pt = await _unitOfWork.PropertyTypeRepository.GetByIdAsync(id);
            if (pt == null) return NotFound();
            var model = new LkpPropertyTypeViewModel { PropertyTypeID = pt.PropertyTypeID, TypeName = pt.TypeName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPropertyType(LkpPropertyTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pt = await _unitOfWork.PropertyTypeRepository.GetByIdAsync(model.PropertyTypeID);
                if (pt != null)
                {
                    pt.TypeName = model.TypeName;
                    _unitOfWork.PropertyTypeRepository.UpdateAsync(pt);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("PropertyTypes");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePropertyType(int id)
        {
            _unitOfWork.PropertyTypeRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("PropertyTypes");
        }

        [HttpGet]
        public async Task<IActionResult> PropertyStatuses()
        {
            ViewBag.PropertyStatuses = (await _unitOfWork.PropertyStatusRepository.ReadAllAsync())
                .Select(ps => new LkpPropertyStatusViewModel { StatusID = ps.StatusID, StatusName = ps.StatusName, Description = ps.Description }).ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CreatePropertyStatus()
        {
            return View(new LkpPropertyStatusViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePropertyStatus(LkpPropertyStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PropertyStatusRepository.AddAsync(new LkpPropertyStatus { StatusName = model.StatusName, Description = model.Description });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("PropertyStatuses");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPropertyStatus(int id)
        {
            var ps = await _unitOfWork.PropertyStatusRepository.GetByIdAsync(id);
            if (ps == null) return NotFound();
            var model = new LkpPropertyStatusViewModel { StatusID = ps.StatusID, StatusName = ps.StatusName, Description = ps.Description };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPropertyStatus(LkpPropertyStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ps = await _unitOfWork.PropertyStatusRepository.GetByIdAsync(model.StatusID);
                if (ps != null)
                {
                    ps.StatusName = model.StatusName;
                    ps.Description = model.Description;
                    _unitOfWork.PropertyStatusRepository.UpdateAsync(ps);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("PropertyStatuses");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePropertyStatus(int id)
        {
            _unitOfWork.PropertyStatusRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("PropertyStatuses");
        }

        [HttpGet]
        public async Task<IActionResult> UserTypes()
        {
            ViewBag.UserTypes = (await _unitOfWork.UserTypeRepository.ReadAllAsync())
                .Select(ut => new LkpUserTypeViewModel { UserTypeID = ut.UserTypeID, UserTypeName = ut.UserTypeName, Description = ut.Description }).ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CreateUserType()
        {
            return View(new LkpUserTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserType(LkpUserTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.UserTypeRepository.AddAsync(new LkpUserType { UserTypeName = model.UserTypeName, Description = model.Description });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("UserTypes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserType(int id)
        {
            var ut = await _unitOfWork.UserTypeRepository.GetByIdAsync(id);
            if (ut == null) return NotFound();
            var model = new LkpUserTypeViewModel { UserTypeID = ut.UserTypeID, UserTypeName = ut.UserTypeName, Description = ut.Description };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserType(LkpUserTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ut = await _unitOfWork.UserTypeRepository.GetByIdAsync(model.UserTypeID);
                if (ut != null)
                {
                    ut.UserTypeName = model.UserTypeName;
                    ut.Description = model.Description;
                    _unitOfWork.UserTypeRepository.UpdateAsync(ut);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("UserTypes");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            _unitOfWork.UserTypeRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("UserTypes");
        }

        #endregion
    }
}