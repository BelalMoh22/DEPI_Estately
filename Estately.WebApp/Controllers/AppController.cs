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

            var model = new PagedPropertyViewModel
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

                model.Properties.Add(new PropertiesListViewModel
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
        public IActionResult PropertySingle() 
        {
            return View();
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
