using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;

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
            // TEMPORARY DEBUGGING
            System.Diagnostics.Debug.WriteLine("=== NEW CONTROLLER CODE IS RUNNING ===");
            Console.WriteLine("=== NEW CONTROLLER CODE IS RUNNING ===");

            try
            {
                var model = await _userTypeService.GetUserTypesPagedAsync(page, pageSize, search);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading user types.";
                return View(new UserTypeListViewModel());
            }
        }

        // GET: LkpUserTypes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var userType = await _userTypeService.GetUserTypeByIdAsync(id);
                if (userType == null)
                {
                    TempData["ErrorMessage"] = "User type not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(userType);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading user type details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: LkpUserTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LkpUserTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!await _userTypeService.IsUserTypeNameUniqueAsync(model.UserTypeName))
                    {
                        ModelState.AddModelError("UserTypeName", "User Type Name already exists.");
                        return View(model);
                    }

                    await _userTypeService.CreateUserTypeAsync(model);
                    TempData["SuccessMessage"] = "User type created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the user type.");
                return View(model);
            }
        }

        // GET: LkpUserTypes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var userType = await _userTypeService.GetUserTypeByIdAsync(id);
                if (userType == null)
                {
                    TempData["ErrorMessage"] = "User type not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(userType);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading user type for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: LkpUserTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserTypeViewModel model)
        {
            try
            {
                if (id != model.UserTypeID)
                {
                    TempData["ErrorMessage"] = "User type ID mismatch.";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    if (!await _userTypeService.IsUserTypeNameUniqueAsync(model.UserTypeName, id))
                    {
                        ModelState.AddModelError("UserTypeName", "User Type Name already exists.");
                        return View(model);
                    }

                    await _userTypeService.UpdateUserTypeAsync(model);
                    TempData["SuccessMessage"] = "User type updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the user type.");
                return View(model);
            }
        }

        // GET: LkpUserTypes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userType = await _userTypeService.GetUserTypeByIdAsync(id);
                if (userType == null)
                {
                    TempData["ErrorMessage"] = "User type not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(userType);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading user type for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: LkpUserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _userTypeService.DeleteUserTypeAsync(id);
                TempData["SuccessMessage"] = "User type deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the user type.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}