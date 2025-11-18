using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;

namespace Estately.WebApp.Controllers
{
    public class TblCitiesController : Controller
    {
        private readonly IServiceCity _serviceCity;

        public TblCitiesController(IServiceCity serviceCity)
        {
            _serviceCity = serviceCity;
        }

        // =======================================================
        // INDEX (LIST + SEARCH + PAGINATION)
        // =======================================================
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _serviceCity.GetCitiesPagedAsync(page, pageSize, search);
            return View(model);
        }

        // =======================================================
        // DETAILS (VIEW CITY INFO)
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _serviceCity.GetCityByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // =======================================================
        // CREATE
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadZonesDropdown();
            return View(new CityViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadZonesDropdown();
                return View(model);
            }
            await _serviceCity.CreateCityAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // EDIT
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _serviceCity.GetCityByIdAsync(id);
            if (model == null)
                return NotFound();

            await LoadZonesDropdown();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadZonesDropdown();
                return View(model);
            }

            // check existence using the existing service method
            var existing = await _serviceCity.GetCityByIdAsync(model.CityID);
            if (existing == null)
                return NotFound();

            await _serviceCity.UpdateCityAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // DELETE (SOFT DELETE)
        // =======================================================
        public async Task<IActionResult> Delete(int id)
        {
            var city = await _serviceCity.GetCityByIdAsync(id);

            if (city == null)
                return NotFound();

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceCity.DeleteCityAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // ASSIGN ZONE
        // =======================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignZone(int cityId, int zoneId)
        {
            await _serviceCity.AssignZoneAsync(cityId, zoneId);
            return RedirectToAction("Edit", new { id = cityId });
        }
        // =======================================================
        // PRIVATE HELPER: LOAD DROPDOWN FOR ZONES
        // =======================================================
        private async Task LoadZonesDropdown()
        {
            var zones = await _serviceCity.GetAllZonesAsync();

            ViewBag.Zones = new SelectList(
                zones,
                "ZoneID",
                "ZoneName"
            );
        }
    }
}