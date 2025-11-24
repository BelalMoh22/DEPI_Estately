using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Estately.Core.Entities;
using Estately.Infrastructure.Data;

namespace Estately.WebApp.Controllers
{
    public class TblZonesController : Controller
    {
        private readonly IServiceZone _serviceZone;

        public TblZonesController(IServiceZone serviceZone)
        {
            _serviceZone = serviceZone;
        }

        // GET: TblZones
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _serviceZone.GetZonePagedAsync(page, pageSize, search);
            return View(model);
        }

        // GET: TblZones/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _serviceZone.GetZoneByIdAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // GET: TblZones/Create
        public async Task<IActionResult> Create()
        {
            var cities = await _serviceZone.GetAllCitiesAsync();
            ViewBag.City = new SelectList(cities, "CityID", "CityName");
            return View(new ZonesViewModel());
        }

        // POST: TblZones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZonesViewModel model)
        {
            // Model-level validation
            if (!ModelState.IsValid)
            {
                ViewBag.City = new SelectList(await _serviceZone.GetAllCitiesAsync(), "CityID", "CityName", model.CityId);
                return View(model);
            }

            // Business validation — prevent duplicate zone names in the same city
            var exists = await _serviceZone.SearchZoneAsync(z =>
                z.ZoneName.ToLower() == model.ZoneName.ToLower() &&
                z.CityID == model.CityId
            );

            if (exists.Any())
            {
                ModelState.AddModelError("ZoneName", "This zone already exists in the selected city.");
                ViewBag.City = new SelectList(await _serviceZone.GetAllCitiesAsync(), "CityID", "CityName", model.CityId);
                return View(model);
            }

            await _serviceZone.CreateZoneAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: TblZones/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _serviceZone.GetZoneByIdAsync(id);
            if (model == null)
                return NotFound();

            var cities = await _serviceZone.GetAllCitiesAsync();
            ViewBag.City = new SelectList(cities, "CityID", "CityName", model.CityId);

            return View(model);
        }

        // POST: TblZones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ZonesViewModel model)
        {
            if (id != model.ZoneId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.City = new SelectList(await _serviceZone.GetAllCitiesAsync(), "CityID", "CityName", model.CityId);
                return View(model);
            }

            // Prevent duplicate zones
            var exists = await _serviceZone.SearchZoneAsync(z =>
                z.ZoneName.ToLower() == model.ZoneName.ToLower() &&
                z.CityID == model.CityId &&
                z.ZoneID != model.ZoneId
            );

            if (exists.Any())
            {
                ModelState.AddModelError("ZoneName", "This zone already exists in the selected city.");
                ViewBag.City = new SelectList(await _serviceZone.GetAllCitiesAsync(), "CityID", "CityName", model.CityId);
                return View(model);
            }

            await _serviceZone.UpdateZoneAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: TblZones/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var zone = await _serviceZone.GetZoneByIdAsync(id);
            if (zone == null)
                return NotFound();

            return View(zone);
        }

        // POST: TblZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _serviceZone.GetZoneByIdAsync(id);
            if (model == null)
                return NotFound();

            // Optional: prevent deletion if a zone is used by properties
            var hasProperties = await _serviceZone.ZoneHasPropertiesAsync(id);
            if (hasProperties)
            {
                TempData["Error"] = "You cannot delete this zone because it contains properties. You must delete those properties first.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            await _serviceZone.DeleteZoneAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
