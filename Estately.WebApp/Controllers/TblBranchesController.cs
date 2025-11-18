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
            var model = new BranchesViewModel();

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
        public async Task<IActionResult> Create(BranchesViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _serviceBranch.CreateBranchAsync(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
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
        public async Task<IActionResult> Edit(int id, BranchesViewModel model)
        {
            if (id != model.BranchID)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                // 🔥 FIX — Repopulate managers dropdown
                var managers = await _serviceBranch.GetAllManagersAsync();
                ViewBag.Managers = managers.Select(m => new SelectListItem
                {
                    Value = $"{m.FirstName} {m.LastName}",
                    Text = $"{m.FirstName} {m.LastName}"
                }).ToList();

                return View(model);
            }

            await _serviceBranch.UpdateBranchAsync(model);

            TempData["Success"] = "Branch updated successfully";
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _serviceBranch.GetBranchByIdAsync(id);
            if (model == null)
                return NotFound();

            await _serviceBranch.DeleteBranchAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
