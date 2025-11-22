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
    public class LkpPropertyStatusController : Controller
    {
        private readonly IServicePropertyStatus _service;

        public LkpPropertyStatusController(IServicePropertyStatus service)
        {
            _service = service;
        }
        // =======================================================
        // INDEX (LIST + SEARCH + PAGINATION)
        // =======================================================
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _service.GetPropertyStatusesPagedAsync(page, pageSize, search);
            return View(model);
        }

        // =======================================================
        // DETAILS (VIEW PROPERTY STATUS INFO)
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _service.GetPropertyStatusByIdAsync(id);

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
            return View(new PropertyStatusViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if status name is unique
            if (!await _service.IsStatusNameUniqueAsync(model.StatusName))
            {
                ModelState.AddModelError("StatusName", "Status name already exists. Please choose a different name.");
                return View(model);
            }

            await _service.CreatePropertyStatusAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // EDIT
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _service.GetPropertyStatusByIdAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if status name is unique (excluding current record)
            if (!await _service.IsStatusNameUniqueAsync(model.StatusName, model.StatusID))
            {
                ModelState.AddModelError("StatusName", "Status name already exists. Please choose a different name.");
                return View(model);
            }

            // Check existence using the existing service method
            var existing = await _service.GetPropertyStatusByIdAsync(model.StatusID);
            if (existing == null)
                return NotFound();

            await _service.UpdatePropertyStatusAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // DELETE
        // =======================================================
        //[HttpGet]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    // Optional: Check if status is being used by any properties before deletion
        //    // var isUsed = await CheckIfStatusIsUsed(id);
        //    // if (isUsed)
        //    // {
        //    //     TempData["ErrorMessage"] = "Cannot delete status because it is being used by properties.";
        //    //     return RedirectToAction(nameof(Index));
        //    // }

        //    await _service.DeletePropertyStatusAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _service.GetPropertyStatusByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: TblDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _service.GetPropertyStatusByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            await _service.DeletePropertyStatusAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // JSON ENDPOINTS FOR AJAX (optional)
        // =======================================================
        [HttpGet]
        public async Task<JsonResult> GetAllPropertyStatusesJson()
        {
            var statuses = await _service.GetAllPropertyStatusesAsync();
            return Json(statuses);
        }

        [HttpGet]
        public async Task<JsonResult> CheckStatusNameUnique(string statusName, int? excludeId = null)
        {
            var isUnique = await _service.IsStatusNameUniqueAsync(statusName, excludeId);
            return Json(new { isUnique });
        }

        // =======================================================
        // PRIVATE HELPER: CHECK IF STATUS IS BEING USED (optional)
        // =======================================================
        //private async Task<bool> CheckIfStatusIsUsed(int statusId)
        //{
        //    // Implement logic to check if any properties are using this status
        //    // This would require a method in your property service/repository
        //    // return await _serviceProperty.CheckIfStatusIsUsedAsync(statusId);
        //    return false;
        //}
    }
    }
