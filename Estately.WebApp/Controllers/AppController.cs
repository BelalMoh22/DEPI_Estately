namespace Estately.WebApp.Controllers
{
    public class AppController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public async Task<IActionResult> Properties(int page = 1, int pageSize = 9)
        {
            var all = await _unitOfWork.PropertyRepository.ReadAllIncluding(
                "Zone",
                "Zone.City",
                "TblPropertyImages"
            );

            int totalCount = all.Count();

            var paged = all
                .Where(p => p.IsDeleted == false)
                .OrderBy(p => p.PropertyID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new PropertyListViewModel
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            foreach (var p in paged)
            {
                // First try: path from TblPropertyImages (new approach)
                var firstImagePath = p.TblPropertyImages?
                    .OrderBy(i => i.ImageID)
                    .Select(i => i.ImagePath)
                    .FirstOrDefault();

                // Normalize to include folder if only file name is stored
                if (!string.IsNullOrWhiteSpace(firstImagePath) && !firstImagePath.Contains('/'))
                {
                    firstImagePath = $"Images/Properties/{firstImagePath}";
                }

                // Fallback for old properties: look for files on disk with legacy pattern
                if (string.IsNullOrWhiteSpace(firstImagePath))
                {
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Properties");
                    string pattern = $"prop-{p.PropertyID}-1.*";

                    var legacy = Directory.Exists(folder)
                        ? Directory.EnumerateFiles(folder, pattern).Select(Path.GetFileName).FirstOrDefault()
                        : null;

                    if (!string.IsNullOrWhiteSpace(legacy))
                    {
                        firstImagePath = $"Images/Properties/{legacy}";
                    }
                }

                // Final fallback
                firstImagePath ??= "Images/Properties/default.jpg";

                model.Properties.Add(new PropertyViewModel
                {
                    PropertyID = p.PropertyID,
                    Address = p.Address,
                    CityName = p.Zone?.City?.CityName ?? "",
                    ZoneName = p.Zone?.ZoneName ?? "",
                    Price = (int)p.Price,
                    BedsNo = p.BedsNo,
                    BathsNo = p.BathsNo,
                    FirstImage = firstImagePath
                });
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
