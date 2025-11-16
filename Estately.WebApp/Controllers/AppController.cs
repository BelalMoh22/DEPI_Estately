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
            // Get the property from the service (or repository)
            var property = await _unitOfWork.PropertyRepository.GetByIdAsync(id);

            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/properties");

            var images = new List<string>();

            for (int i = 1; i <= 3; i++)
            {
                string pattern = $"prop-{property.PropertyID}-{i}.*";

                var image = Directory
                    .EnumerateFiles(folder, pattern)
                    .Select(Path.GetFileName)
                    .FirstOrDefault();

                images.Add(image);
            }

            var firstImage = images.ElementAtOrDefault(0);
            var secondImage = images.ElementAtOrDefault(1);
            var thirdImage = images.ElementAtOrDefault(2);

            if (property == null)
                return NotFound();

            // Map Property → SinglePropertyViewModel
            var Model = new SinglePropertyViewModel
            {
                PropertyID = property.PropertyID,
                PropertyCode = property.PropertyCode,
                PropertyType = property.PropertyType,
                Address = property.Address,
                CityName = property.Zone?.City?.CityName ?? "",
                ZoneName = property.Zone?.ZoneName ?? "",
                Price = property.Price,
                Beds = property.BedsNo,
                Baths = property.BathsNo,
                Area = property.Area,
                FirstImage = firstImage,
                SecondImage = secondImage,
                ThirdImage = thirdImage
            };

            return View(Model);
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
