using Estately.Core.Entities;
using Estately.Core.Entities.Identity;
using Estately.Core.Interfaces;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace Estately.WebApp.Controllers
{
    public class AppController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProperty _serviceProperty;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public AppController(IUnitOfWork unitOfWork, IServiceProperty serviceProperty, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _serviceProperty = serviceProperty;
            _userManager = userManager;
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Blog() 
        { 
            return View();
        }
        public IActionResult Contact() 
        {
            return View();
        }
        public IActionResult Services() 
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Properties(
            int page = 1, 
            int pageSize = 9,
            string? search = null,
            string? city = null,
            string? areas = null,
            string? zones = null,
            string? developers = null,
            string? propertyTypes = null,
            string? minPrice = null,
            string? maxPrice = null,
            string? minArea = null,
            string? maxArea = null,
            string? bedrooms = null,
            string? bathrooms = null,
            string? amenities = null)
        {
            // Normalize empty strings to null for optional parameters
            if (string.IsNullOrWhiteSpace(search)) search = null;
            if (string.IsNullOrWhiteSpace(city)) city = null;
            if (string.IsNullOrWhiteSpace(areas)) areas = null;
            if (string.IsNullOrWhiteSpace(zones)) zones = null;
            if (string.IsNullOrWhiteSpace(developers)) developers = null;
            if (string.IsNullOrWhiteSpace(propertyTypes)) propertyTypes = null;
            if (string.IsNullOrWhiteSpace(amenities)) amenities = null;
            
            // Parse filter parameters (all optional)
            decimal? minPriceDecimal = null;
            decimal? maxPriceDecimal = null;
            decimal? minAreaDecimal = null;
            decimal? maxAreaDecimal = null;
            int? bedroomsInt = null;
            int? bathroomsInt = null;

            if (!string.IsNullOrWhiteSpace(minPrice) && decimal.TryParse(minPrice, out var minP))
                minPriceDecimal = minP;
            
            if (!string.IsNullOrWhiteSpace(maxPrice) && decimal.TryParse(maxPrice, out var maxP))
                maxPriceDecimal = maxP;
            
            if (!string.IsNullOrWhiteSpace(minArea) && decimal.TryParse(minArea, out var minA))
                minAreaDecimal = minA;
            
            if (!string.IsNullOrWhiteSpace(maxArea) && decimal.TryParse(maxArea, out var maxA))
                maxAreaDecimal = maxA;

            // Parse bedrooms/bathrooms (handle "5+" case)
            if (!string.IsNullOrWhiteSpace(bedrooms))
            {
                if (bedrooms.Trim() == "5+")
                    bedroomsInt = 5; // Will be handled as >= 5 in service
                else if (int.TryParse(bedrooms, out var beds))
                    bedroomsInt = beds;
            }

            if (!string.IsNullOrWhiteSpace(bathrooms))
            {
                if (bathrooms.Trim() == "5+")
                    bathroomsInt = 5; // Will be handled as >= 5 in service
                else if (int.TryParse(bathrooms, out var baths))
                    bathroomsInt = baths;
            }

            // Ensure valid pagination parameters
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 9 : pageSize;

            // Use filtered service method (all filters are optional)
            var model = await _serviceProperty.GetPropertiesFilteredAsync(
                page,
                pageSize,
                search,
                city,
                zones, // Now includes areas
                developers,
                propertyTypes,
                minPriceDecimal,
                maxPriceDecimal,
                minAreaDecimal,
                maxAreaDecimal,
                bedroomsInt,
                bathroomsInt,
                amenities
            );

            // Process images for display
            foreach (var property in model.Properties)
            {
                if (string.IsNullOrWhiteSpace(property.FirstImage) || property.FirstImage == "default.jpg")
                {
                    // Try to get first image from property images
                    var prop = await _unitOfWork.PropertyRepository.GetByIdIncludingAsync(
                        property.PropertyID,
                        "TblPropertyImages"
                    );

                    if (prop?.TblPropertyImages != null && prop.TblPropertyImages.Any())
                    {
                        var firstImg = prop.TblPropertyImages
                            .OrderBy(i => i.ImageID)
                            .FirstOrDefault();
                        
                        if (firstImg != null)
                        {
                            property.FirstImage = firstImg.ImagePath.Contains('/') 
                                ? firstImg.ImagePath 
                                : $"Images/Properties/{firstImg.ImagePath}";
                        }
                    }

                    // Final fallback
                    if (string.IsNullOrWhiteSpace(property.FirstImage) || property.FirstImage == "default.jpg")
                    {
                        property.FirstImage = "Images/Properties/default.jpg";
                    }
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> PropertySingle(int id)
        {
            var property = await _unitOfWork.PropertyRepository
                .GetByIdIncludingAsync(
                    id,
                    "TblPropertyImages",
                    "Zone",
                    "Zone.City",
                    "PropertyType",
                    "Status"
                );

            if (property == null)
                return NotFound();

            var model = new SinglePropertyViewModel
            {
                PropertyID = property.PropertyID,
                PropertyCode = property.PropertyCode,
                Description = property.Description ?? "",
                Address = property.Address,
                PropertyTypeName = property.PropertyType?.TypeName ?? "",
                PropertyStatusName = property.Status?.StatusName ?? "",
                CityName = property.Zone?.City?.CityName ?? "",
                ZoneName = property.Zone?.ZoneName ?? "",
                Price = (int) property.Price,
                Beds = property.BedsNo,
                Baths = property.BathsNo,
                FloorNo = property.FloorNo ?? null,
                Area = property.Area,
                ExpectedRent = (int?)property.ExpectedRentPrice ?? 0,
                Latitude = property.Latitude,
                Longitude = property.Longitude,

                Images = property.TblPropertyImages
                    .OrderBy(i => i.ImageID)
                    .Select(i =>
                    {
                        if (string.IsNullOrWhiteSpace(i.ImagePath)) return null;
                        return i.ImagePath.Contains('/') ? i.ImagePath : $"Images/Properties/{i.ImagePath}";
                    })
                    .Where(p => p != null)
                    .Cast<string>()
                    .ToList()
            };

            // Fallback for legacy properties with images only on disk
            if (model.Images.Count == 0)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Properties");
                string pattern = $"prop-{property.PropertyID}-*.*";

                if (Directory.Exists(folder))
                {
                    var legacyFiles = Directory.EnumerateFiles(folder, pattern)
                        .Select(Path.GetFileName)
                        .ToList();

                    foreach (var file in legacyFiles)
                    {
                        model.Images.Add($"Images/Properties/{file}");
                    }
                }
            }

            return View(model);
        }

        public IActionResult MyAccount() 
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ClientAccount(bool edit = false)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (appUser == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var profile = await _unitOfWork.ClientProfileRepository
                .Query()
                .FirstOrDefaultAsync(p => p.UserID == appUser.Id);

            var model = new ClientProfileViewModel
            {
                ClientProfileID = profile?.ClientProfileID ?? 0,
                UserID = appUser.Id,
                FirstName = profile?.FirstName,
                LastName = profile?.LastName,
                Phone = profile?.Phone,
                Address = profile?.Address,
                ProfilePhoto = profile?.ProfilePhoto,
                Username = appUser.UserName
            };

            ViewBag.EditMode = edit;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveClientAccount(ClientProfileViewModel model)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (appUser == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            if (!ModelState.IsValid)
            {
                model.Username = appUser.UserName;
                return View("ClientAccount", model);
            }

            var profile = await _unitOfWork.ClientProfileRepository
                .Query()
                .FirstOrDefaultAsync(p => p.UserID == appUser.Id);

            if (profile == null)
            {
                profile = new TblClientProfile
                {
                    UserID = appUser.Id
                };

                await _unitOfWork.ClientProfileRepository.AddAsync(profile);
            }

            profile.FirstName = model.FirstName;
            profile.LastName = model.LastName;
            profile.Phone = model.Phone;
            profile.Address = model.Address;

            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(ClientAccount), new { edit = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadClientPhoto(IFormFile photo)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (appUser == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "Images", "Profiles");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"client-{appUser.Id}-{DateTime.UtcNow.Ticks}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                var relativePath = "/" + Path.Combine("Images", "Profiles", fileName).Replace("\\", "/");

                var profile = await _unitOfWork.ClientProfileRepository
                    .Query()
                    .FirstOrDefaultAsync(p => p.UserID == appUser.Id);

                if (profile == null)
                {
                    profile = new TblClientProfile
                    {
                        UserID = appUser.Id
                    };

                    await _unitOfWork.ClientProfileRepository.AddAsync(profile);
                }

                profile.ProfilePhoto = relativePath;
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(ClientAccount), new { edit = false });
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeAccount(bool edit = false)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (appUser == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var employee = await _unitOfWork.EmployeeRepository
                .Query()
                .FirstOrDefaultAsync(e => e.UserID == appUser.Id);

            var model = new EmployeeViewModel
            {
                EmployeeID = employee?.EmployeeID ?? 0,
                UserID = appUser.Id,
                FirstName = employee?.FirstName ?? string.Empty,
                LastName = employee?.LastName ?? string.Empty,
                Gender = employee?.Gender ?? string.Empty,
                Age = employee?.Age.ToString() ?? string.Empty,
                Phone = employee?.Phone ?? string.Empty,
                Nationalid = employee?.Nationalid ?? string.Empty,
                ProfilePhoto = employee?.ProfilePhoto,
                Username = appUser.UserName
            };

            ViewBag.EditMode = edit;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEmployeeAccount(EmployeeViewModel model)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (appUser == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            if (!ModelState.IsValid)
            {
                model.Username = appUser.UserName;
                ViewBag.EditMode = true;
                return View("EmployeeAccount", model);
            }

            var employee = await _unitOfWork.EmployeeRepository
                .Query()
                .FirstOrDefaultAsync(e => e.UserID == appUser.Id);

            if (employee == null)
            {
                employee = new TblEmployee
                {
                    UserID = appUser.Id
                };

                await _unitOfWork.EmployeeRepository.AddAsync(employee);
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Gender = model.Gender;
            if (int.TryParse(model.Age, out var ageInt))
            {
                employee.Age = ageInt;
            }
            employee.Phone = model.Phone;
            employee.Nationalid = model.Nationalid;

            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(EmployeeAccount), new { edit = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadEmployeePhoto(IFormFile photo)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (appUser == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "Images", "Profiles");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"employee-{appUser.Id}-{DateTime.UtcNow.Ticks}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                var relativePath = "/" + Path.Combine("Images", "Profiles", fileName).Replace("\\", "/");

                var employee = await _unitOfWork.EmployeeRepository
                    .Query()
                    .FirstOrDefaultAsync(e => e.UserID == appUser.Id);

                if (employee == null)
                {
                    employee = new TblEmployee
                    {
                        UserID = appUser.Id
                    };

                    await _unitOfWork.EmployeeRepository.AddAsync(employee);
                }

                employee.ProfilePhoto = relativePath;
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(EmployeeAccount), new { edit = false });
        }

        [HttpGet]
        public async Task<IActionResult> DeveloperAccount(bool edit = false)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (appUser == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var dev = await _unitOfWork.DeveloperProfileRepository
                .Query()
                .FirstOrDefaultAsync(d => d.UserID == appUser.Id);

            var model = new DeveloperProfileViewModel
            {
                DeveloperProfileID = dev?.DeveloperProfileID ?? 0,
                UserID = appUser.Id,
                DeveloperTitle = dev?.DeveloperTitle ?? string.Empty,
                WebsiteURL = dev?.WebsiteURL,
                PortofolioPhoto = dev?.PortofolioPhoto,
                Phone = dev?.Phone,
                Username = appUser.UserName
            };

            ViewBag.EditMode = edit;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDeveloperAccount(DeveloperProfileViewModel model)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (appUser == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            if (!ModelState.IsValid)
            {
                model.Username = appUser.UserName;
                ViewBag.EditMode = true;
                return View("DeveloperAccount", model);
            }

            var dev = await _unitOfWork.DeveloperProfileRepository
                .Query()
                .FirstOrDefaultAsync(d => d.UserID == appUser.Id);

            if (dev == null)
            {
                dev = new TblDeveloperProfile
                {
                    UserID = appUser.Id
                };

                await _unitOfWork.DeveloperProfileRepository.AddAsync(dev);
            }

            dev.DeveloperTitle = model.DeveloperTitle;
            dev.WebsiteURL = model.WebsiteURL;
            dev.Phone = model.Phone;

            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(DeveloperAccount), new { edit = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDeveloperPhoto(IFormFile photo)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var appUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            if (appUser == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "Images", "Profiles");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"developer-{appUser.Id}-{DateTime.UtcNow.Ticks}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                var relativePath = "/" + Path.Combine("Images", "Profiles", fileName).Replace("\\", "/");

                var dev = await _unitOfWork.DeveloperProfileRepository
                    .Query()
                    .FirstOrDefaultAsync(d => d.UserID == appUser.Id);

                if (dev == null)
                {
                    dev = new TblDeveloperProfile
                    {
                        UserID = appUser.Id
                    };

                    await _unitOfWork.DeveloperProfileRepository.AddAsync(dev);
                }

                dev.PortofolioPhoto = relativePath;
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(DeveloperAccount), new { edit = false });
        }
        public IActionResult Favorites() 
        {
            return View();
        }
        public IActionResult Advertise() 
        {
            return View();
        }
    }
}
