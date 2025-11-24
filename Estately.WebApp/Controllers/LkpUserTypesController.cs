using Estately.Services.Implementations;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Estately.WebApp.Controllers
{
    public class LkpUserTypesController : Controller
    {
        private readonly IServiceUserType _userTypeService;

        public LkpUserTypesController(IServiceUserType userTypeService)
        {
            _userTypeService = userTypeService;
        }
        // GET: LkpUserTypes
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _userTypeService.GetUserTypesPagedAsync(page, pageSize, search);
            return View(model);
        }

        // GET: LkpUserTypes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _userTypeService.GetUserTypeByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // GET: LkpUserTypes/Create
        public async Task<IActionResult> Create()
        {
            return View(new UserTypeViewModel());
        }

        // POST: LkpUserTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!await _userTypeService.IsUserTypeNameUniqueAsync(model.UserTypeName))
            {
                ModelState.AddModelError("UserTypeName", "User Type Name already exists.");
                return View(model);
            }

            await _userTypeService.CreateUserTypeAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: LkpUserTypes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _userTypeService.GetUserTypeByIdAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: LkpUserTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserTypeViewModel model)
        {
            if (id != model.UserTypeID)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            if (!await _userTypeService.IsUserTypeNameUniqueAsync(model.UserTypeName, model.UserTypeID))
            {
                ModelState.AddModelError("UserTypeName", "This user type name already exists.");
                return View(model);
            }

            await _userTypeService.UpdateUserTypeAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: LkpUserTypes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _userTypeService.GetUserTypeByIdAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: LkpUserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _userTypeService.GetUserTypeByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            if (await _userTypeService.IsTypeUsedAsync(id))
            {
                TempData["Error"] = "Cannot delete this usertype because it contains users. You must delete those users first.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            await _userTypeService.DeleteUserTypeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}