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
    public class TblPropertiesController : Controller
    {
        private readonly IServiceProperty _service;

        public TblPropertiesController(IServiceProperty service)
        {
            _service = service;
        }

        // -------------------------------------------------------
        // INDEX (Paged + Search)
        // -------------------------------------------------------
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _service.GetPropertiesPagedAsync(page, pageSize, search);
            return View(model);
        }

        // -------------------------------------------------------
        // CREATE (GET)
        // -------------------------------------------------------
        public async Task<IActionResult> Create()
        {
            await LoadLookups();
            return View(new PropertyViewModel());
        }

        // -------------------------------------------------------
        // CREATE (POST)
        // -------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookups();
                return View(vm);
            }

            await _service.CreatePropertyAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        // -------------------------------------------------------
        // EDIT (GET)
        // -------------------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetPropertyByIdAsync(id);
            if (vm == null) return NotFound();

            await LoadLookups();
            return View(vm);
        }

        // -------------------------------------------------------
        // EDIT (POST)
        // -------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookups();
                return View(vm);
            }

            await _service.UpdatePropertyAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        // -------------------------------------------------------
        // DETAILS
        // -------------------------------------------------------
        public async Task<IActionResult> Details(int id)
        {
            var vm = await _service.GetPropertyByIdAsync(id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        // -------------------------------------------------------
        // DELETE
        // -------------------------------------------------------
        // GET: Property/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _service.GetPropertyByIdAsync(id);

            if (property == null)
                return NotFound();

            return View(property);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeletePropertyAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // -------------------------------------------------------
        // LOAD LOOKUPS FOR DROPDOWNS
        // -------------------------------------------------------
        private async Task LoadLookups()
        {
            ViewBag.PropertyTypes = new SelectList(await _service.GetAllPropertyTypesAsync(), "PropertyTypeID", "TypeName");
            ViewBag.Statuses = new SelectList(await _service.GetAllStatusesAsync(), "StatusID", "StatusName");
            ViewBag.Developers = new SelectList(await _service.GetAllDevelopersAsync(), "DeveloperProfileID", "DeveloperName");
            ViewBag.Zones = new SelectList(await _service.GetAllZonesAsync(), "ZoneId", "ZoneName");

            var agents = await _service.GetAgentsAsync(); 
            ViewBag.Agents = agents
                .Select(a => new SelectListItem
                {
                    Value = a.EmployeeID.ToString(),
                    Text = a.FirstName + " " + a.LastName
                })
                .ToList();
        }
    }
}