using Microsoft.AspNetCore.Mvc.Rendering;
namespace Estately.WebApp.Controllers
{
    public class TblUsersController : Controller
    {
        private readonly IServiceUser _serviceUser;

        public TblUsersController(IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
        }
        // =======================================================
        // INDEX (LIST + SEARCH + PAGINATION)
        // =======================================================
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _serviceUser.GetUsersPagedAsync(page, pageSize, search);
            return View(model);
        }
        // =======================================================
        // DETAILS (VIEW USER INFO)
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _serviceUser.GetUserByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // =======================================================
        // EDIT
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _serviceUser.GetUserByIdAsync(id);
            if (model == null)
                return NotFound();

            await LoadUserTypesDropdown();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadUserTypesDropdown();
                return View(model);
            }

            // check existence using the existing service method
            var existing = await _serviceUser.GetUserByIdAsync(model.UserID);
            if (existing == null)
                return NotFound();

            await _serviceUser.UpdateUserAsync(model);
            TempData["Success"] = "User updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // DELETE (SOFT DELETE)
        // =======================================================
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _serviceUser.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceUser.DeleteUserAsync(id);
            TempData["Success"] = "User deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // TOGGLE ACTIVE / INACTIVE
        // =======================================================
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ToggleStatus(int id)
        //{
        //    await _serviceUser.ToggleStatusAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}

        // =======================================================
        // ASSIGN ROLE
        // =======================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(int userId, int userTypeId)
        {
            await _serviceUser.AssignRoleAsync(userId, userTypeId);
            return RedirectToAction("Edit", new { id = userId });
        }

        // =======================================================
        // PRIVATE HELPER: LOAD DROPDOWN FOR USER TYPES
        // =======================================================
        private async Task LoadUserTypesDropdown()
        {
            var userTypes = await _serviceUser.GetAllUserTypesAsync();

            ViewBag.UserTypes = new SelectList(
                userTypes,
                "UserTypeID",
                "UserTypeName"
            );
        }
    }
}