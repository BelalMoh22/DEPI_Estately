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
    public class TblPropertyFeaturesController : Controller
    {
        private readonly IServicePropertyFeature _service;

        public TblPropertyFeaturesController(IServicePropertyFeature service)
        {
            _service = service;
        }

        // INDEX
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _service.GetPropertyFeaturesPagedAsync(page, pageSize, search);
            return View(model);
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var model = await _service.GetPropertyFeatureByIdAsync(id);
            return model == null ? NotFound() : View(model);
        }

        // CREATE GET
        public IActionResult Create()
            => View(new PropertyFeatureViewModel());

        // CREATE POST
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!await _service.IsFeatureNameUniqueAsync(model.FeatureName))
            {
                ModelState.AddModelError("FeatureName", "Feature name already exists.");
                return View(model);
            }

            await _service.CreatePropertyFeatureAsync(model);
            TempData["Success"] = "Feature created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _service.GetPropertyFeatureByIdAsync(id);
            return model == null ? NotFound() : View(model);
        }

        // EDIT POST
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!await _service.IsFeatureNameUniqueAsync(model.FeatureName, model.FeatureID))
            {
                ModelState.AddModelError("FeatureName", "Feature name already exists.");
                return View(model);
            }

            await _service.UpdatePropertyFeatureAsync(model);
            TempData["Success"] = "Feature updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _service.GetPropertyFeatureByIdAsync(id);
            return model == null ? NotFound() : View(model);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _service.IsFeatureUsedAsync(id))
            {
                TempData["Error"] = "Cannot delete this feature because it is used in properties.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            await _service.DeletePropertyFeatureAsync(id);
            TempData["Success"] = "Feature deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
