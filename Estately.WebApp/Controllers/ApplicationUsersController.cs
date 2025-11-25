using Estately.Core.Interfaces;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estately.WebApp.Controllers
{
    [Authorize]
    public class ApplicationUsersController : Controller
    {
        private readonly IServiceUser _serviceUser;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationUsersController(IServiceUser serviceUser, IUnitOfWork unitOfWork)
        {
            _serviceUser = serviceUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var vm = await _serviceUser.GetUsersPagedAsync(page, pageSize, search);
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vm = await _serviceUser.GetUserByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _serviceUser.GetUserByIdAsync(id);
            if (vm == null) return NotFound();

            await BuildUserTypesAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await BuildUserTypesAsync(vm);
                return View(vm);
            }

            await _serviceUser.UpdateUserAsync(vm);
            TempData["Success"] = "User updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vm = await _serviceUser.GetUserByIdAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vm = await _serviceUser.GetUserByIdAsync(id);
            if (vm == null) return NotFound();

            if (vm.UserTypeID == 4)
            {
                TempData["Error"] = "Admin users cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }

            await _serviceUser.DeleteUserAsync(id);
            TempData["Success"] = "User deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task BuildUserTypesAsync(ApplicationUserViewModel vm)
        {
            var userTypes = await _unitOfWork.UserTypeRepository.ReadAllAsync();
            vm.UserTypes = userTypes.Select(u => new UserTypeViewModel
            {
                UserTypeID = u.UserTypeID,
                UserTypeName = u.UserTypeName
            });
        }
    }
}
