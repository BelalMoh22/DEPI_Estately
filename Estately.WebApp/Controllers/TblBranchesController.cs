using Estately.Services.Implementations;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace Estately.WebApp.Controllers
{
    public class TblBranchesController : Controller
    {
        private readonly IServiceBranch _serviceBranch;

        public TblBranchesController(IServiceBranch serviceBranch)
        {
            _serviceBranch = serviceBranch;
        }

        // ===============================
        // GET: TblBranches
        // ===============================
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _serviceBranch.GetBranchPagedAsync(page, pageSize, search);
            return View(model);
        }

        // ===============================
        // GET: TblBranches/Details/5
        // ===============================
        public async Task<IActionResult> Details(int? id)
        {
            var model = await _serviceBranch.GetBranchByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // ===============================
        // GET: TblBranches/Create
        // ===============================
        public async Task<IActionResult> Create()
        {
            var model = new BranchViewModel();

            var managers = await _serviceBranch.GetAllManagersAsync();

            ViewBag.Managers = managers.Select(m => new SelectListItem
                {
                    Value = $"{m.FirstName} {m.LastName}",
                    Text = $"{m.FirstName} {m.LastName}"
                })
                .ToList();

            return View(model);
        }


        // ===============================
        // POST: TblBranches/Create
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchViewModel model)
        {
            if (await _serviceBranch.BranchNameExistsAsync(model.BranchName, null))
                ModelState.AddModelError("BranchName", "This branch name already exists.");

            if (!string.IsNullOrEmpty(model.ManagerName) &&
                await _serviceBranch.ManagerAssignedElsewhereAsync(model.ManagerName, null))
                ModelState.AddModelError("ManagerName", "This manager is already assigned to another branch.");

            if (await _serviceBranch.BranchPhoneExistsAsync(model.Phone, null))
                ModelState.AddModelError("Phone", "This phone number already exists for another branch.");

            if (!ModelState.IsValid)
            {
                var managers = await _serviceBranch.GetAllManagersAsync();
                ViewBag.Managers = managers.Select(m => new SelectListItem
                {
                    Value = $"{m.FirstName} {m.LastName}",
                    Text = $"{m.FirstName} {m.LastName}"
                }).ToList();

                return View(model);
            }

            await _serviceBranch.CreateBranchAsync(model);
            return RedirectToAction(nameof(Index));
        }


        // ===============================
        // GET: TblBranches/Edit/5
        // ===============================
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _serviceBranch.GetBranchByIdAsync(id);
            if (model == null)
                return NotFound();

            var managers = await _serviceBranch.GetAllManagersAsync();

            ViewBag.Managers = managers.Select(m => new SelectListItem
            {
                Value = $"{m.FirstName} {m.LastName}",
                Text = $"{m.FirstName} {m.LastName}",
                Selected = $"{m.FirstName} {m.LastName}" == model.ManagerName
            }).ToList();

            return View(model);
        }

        // ===============================
        // POST: TblBranches/Edit/5
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BranchViewModel model)
        {
            if (id != model.BranchID)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            // 🔥 UNIQUE BRANCH NAME CHECK ON UPDATE
            if (await _serviceBranch.BranchNameExistsAsync(model.BranchName, model.BranchID))
            {
                ModelState.AddModelError("BranchName", "This branch name already exists.");
                return View(model);
            }

            // 🔥 CHECK IF MANAGER IS ALREADY ASSIGNED TO ANOTHER BRANCH (EXCLUDING THIS ONE)
            if (!string.IsNullOrEmpty(model.ManagerName) &&
                await _serviceBranch.ManagerAssignedElsewhereAsync(model.ManagerName, model.BranchID))
            {
                ModelState.AddModelError("ManagerName", "This manager is already assigned to another branch.");
                return View(model);
            }

            // 🔥 UNIQUE PHONE CHECK ON UPDATE
            if (await _serviceBranch.BranchPhoneExistsAsync(model.Phone, model.BranchID))
            {
                ModelState.AddModelError("Phone", "This phone number already exists for another branch.");
                return View(model);
            }

            await _serviceBranch.UpdateBranchAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // GET: TblBranches/Delete/5
        // ===============================
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _serviceBranch.GetBranchByIdAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // ===============================
        // POST: TblBranches/Delete/5
        // ===============================
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _serviceBranch.GetBranchByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            if (await _serviceBranch.BranchHasEmployeesAsync(id))
            {
                TempData["Error"] = "Cannot delete this branch because it contains employees. You must delete those employees first.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            await _serviceBranch.DeleteBranchAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
