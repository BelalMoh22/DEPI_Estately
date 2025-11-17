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
            if (ModelState.IsValid)
            {
                await _serviceZone.CreateZoneAsync(model);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.City = new SelectList(await _serviceZone.GetAllCitiesAsync(), "CityId", "CityName", model.CityId);
            return View(model);
        }

        // GET: TblZones/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _serviceZone.GetZoneByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

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

            if (ModelState.IsValid)
            {
                await _serviceZone.UpdateZoneAsync(model);
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown when validation fails
            var cities = await _serviceZone.GetAllCitiesAsync();
            ViewBag.City = new SelectList(cities, "CityID", "CityName", model.CityId);

            return View(model);
        }

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
            {
                return NotFound();
            }

            await _serviceZone.DeleteZoneAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
