using Estately.Services.ViewModels;

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
                "Zone.City"
            );

            int totalCount = all.Count();

            var paged = all
                .OrderBy(p => p.PropertyID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new PropertiesListViewModel
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/properties");

            foreach (var p in paged)
            {
                // Load only FIRST image
                string firstImagePattern = $"prop-{p.PropertyID}-1.*";

                var firstImage = Directory
                    .EnumerateFiles(folder, firstImagePattern)
                    .Select(Path.GetFileName)
                    .FirstOrDefault();

                if (firstImage == null)
                    firstImage = "default.jpg";

                model.Properties.Add(new PropertiesViewModel
                {
                    PropertyID = p.PropertyID,
                    Address = p.Address,
                    CityName = p.Zone?.City?.CityName ?? "",
                    ZoneName = p.Zone?.ZoneName ?? "",
                    Price = (int)p.Price,
                    Beds = p.BedsNo,
                    Baths = p.BathsNo,
                    FirstImage = firstImage
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
                FloorsNo = property.FloorsNo,
                Area = property.Area,
                ExpectedRent = (int?)property.ExpectedRentPrice ?? 0,
                Latitude = property.Latitude,
                Longitude = property.Longitude,

                Images = property.TblPropertyImages
                    .Where(i => i.IsDeleted == false)
                    .OrderBy(i => i.ImageID)
                    .Select(i => i.ImagePath)
                    .ToList()
            };

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
