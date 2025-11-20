using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Estately.Core.Entities;
using Estately.Infrastructure.Data;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;

namespace Estately.WebApp.Controllers
{
    public class LkpPropertyTypesController : Controller
    {
        private readonly IServicePropertyType _servicePropertyType;

        public LkpPropertyTypesController(IServicePropertyType servicePropertyType)
        {
            _servicePropertyType = servicePropertyType;
        }

        // GET: LkpPropertyTypes
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _servicePropertyType.GetPropertyTypesPagedAsync(page, pageSize, search);
            return View(model);
        }

        // GET: LkpPropertyTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _servicePropertyType.GetPropertyTypeByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: LkpPropertyTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LkpPropertyTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _servicePropertyType.CreatePropertyTypeAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: LkpPropertyTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _servicePropertyType.GetPropertyTypeByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: LkpPropertyTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PropertyTypeViewModel model)
        {
            if (id != model.PropertyTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _servicePropertyType.UpdatePropertyTypeAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: LkpPropertyTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _servicePropertyType.GetPropertyTypeByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: LkpPropertyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _servicePropertyType.DeletePropertyTypeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}