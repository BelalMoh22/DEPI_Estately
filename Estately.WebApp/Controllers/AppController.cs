using Estately.Services.Interfaces;

namespace Estately.WebApp.Controllers
{
    public class AppController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProperty _serviceProperty;

        public AppController(IUnitOfWork unitOfWork, IServiceProperty serviceProperty)
        {
            _unitOfWork = unitOfWork;
            _serviceProperty = serviceProperty;
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
        public IActionResult ClientAccount()
        {
            return View();
        }
        public IActionResult EmployeeAccount()
        {
            return View();
        }
        public IActionResult DeveloperAccount()
        {
            return View();
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
