using Estately.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estately.WebApp.Controllers
{
    public class TblDepartmentsController : Controller
    {
        private readonly IServiceDepartment _serviceDep;

        public TblDepartmentsController(IServiceDepartment serviceDep)
        {
            _serviceDep = serviceDep;
        }

        // GET: TblDepartments
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _serviceDep.GetDepartmentPagedAsync(page, pageSize, search);
            return View(model);
        }

        // GET: TblDepartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var model = await _serviceDep.GetDepartmentByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // GET: TblDepartments/Create
        public async Task<IActionResult> Create()
        {
            var employees = await _serviceDep.GetAllManagersAsync();

            ViewBag.Managers = employees
                .Select(e => new SelectListItem
                {
                    Value = $"{e.FirstName} {e.LastName}",
                    Text = $"{e.FirstName} {e.LastName}"
                })
                .ToList();

            return View(new DepartmentsViewModel());
        }



        // POST: TblDepartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadManagersAsync();
                return View(model);
            }

            // 1) Duplicate Name
            if (await _serviceDep.DepartmentNameExistsAsync(model.DepartmentName, null))
            {
                ModelState.AddModelError("DepartmentName", "This department already exists.");
                await LoadManagersAsync();
                return View(model);
            }

            // 2) Manager already assigned
            if (!string.IsNullOrEmpty(model.ManagerName) &&
                await _serviceDep.ManagerAssignedAsync(model.ManagerName, null))
            {
                ModelState.AddModelError("ManagerName", "This manager is already assigned to another department.");
                await LoadManagersAsync();
                return View(model);
            }

            // 3) Email unique
            if (!string.IsNullOrEmpty(model.Email) &&
                await _serviceDep.EmailExistsAsync(model.Email, null))
            {
                ModelState.AddModelError("Email", "This email is already used by another department.");
                await LoadManagersAsync();
                return View(model);
            }

            await _serviceDep.CreateDepartmentAsync(model);
            return RedirectToAction(nameof(Index));
        }



        // GET: TblDepartments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _serviceDep.GetDepartmentByIdAsync(id);
            if (model == null) return NotFound();

            var employees = await _serviceDep.GetAllManagersAsync();
            ViewBag.Managers = employees.Select(e => new SelectListItem
            {
                Value = $"{e.FirstName} {e.LastName}",
                Text = $"{e.FirstName} {e.LastName}",
                Selected = model.ManagerName == $"{e.FirstName} {e.LastName}"
            }).ToList();

            return View(model);
        }


        // POST: TblDepartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentsViewModel model)
        {
            if (id != model.DepartmentID)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadManagersAsync();
                return View(model);
            }

            // 1) Duplicate name (excluding current)
            if (await _serviceDep.DepartmentNameExistsAsync(model.DepartmentName, model.DepartmentID))
            {
                ModelState.AddModelError("DepartmentName", "Another department with this name already exists.");
                await LoadManagersAsync();
                return View(model);
            }

            // 2) Manager assigned elsewhere
            if (!string.IsNullOrEmpty(model.ManagerName) &&
                await _serviceDep.ManagerAssignedAsync(model.ManagerName, model.DepartmentID))
            {
                ModelState.AddModelError("ManagerName", "This manager is already assigned to another department.");
                await LoadManagersAsync();
                return View(model);
            }

            // 3) Unique email on edit
            if (!string.IsNullOrEmpty(model.Email) &&
                await _serviceDep.EmailExistsAsync(model.Email, model.DepartmentID))
            {
                ModelState.AddModelError("Email", "This email is already used by another department.");
                await LoadManagersAsync();
                return View(model);
            }

            await _serviceDep.UpdateDepartmentAsync(model);
            return RedirectToAction(nameof(Index));
        }



        // GET: TblDepartments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _serviceDep.GetDepartmentByIdAsync(id);
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
            // Prevent delete if employees exist
            if (await _serviceDep.HasEmployeesAsync(id))
            {
                TempData["Error"] = "Cannot delete this department because it has assigned employees.";
                return RedirectToAction(nameof(Index));
            }

            await _serviceDep.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task LoadManagersAsync()
        {
            var managers = await _serviceDep.GetAllManagersAsync();
            ViewBag.Managers = managers.Select(m => new SelectListItem
            {
                Text = m.FirstName + " " + m.LastName,
                Value = m.FirstName + " " + m.LastName
            });
        }

    }
}
