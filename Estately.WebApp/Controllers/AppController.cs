using Microsoft.AspNetCore.Mvc;

namespace Estately.WebApp.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
