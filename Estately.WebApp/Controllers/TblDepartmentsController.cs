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
            var model = new DepartmentsViewModel();

            var employees = await _serviceDep.GetAllManagersAsync();

            ViewBag.Managers = employees.Select(e => new SelectListItem
            {
                Value = e.FirstName + " " + e.LastName,
                Text = e.FirstName + " " + e.LastName
            }).ToList();

            return View(model);
        }


        // POST: TblDepartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentsViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _serviceDep.CreateDepartmentAsync(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: TblDepartments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _serviceDep.GetDepartmentByIdAsync(id);

            var managers = await _serviceDep.GetAllManagersAsync();

            ViewBag.Managers = new SelectList(
                managers,
                "FirstName",  // or FullName if you add property
                "FirstName"
            );

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

            if (ModelState.IsValid)
            {
                await _serviceDep.UpdateDepartmentAsync(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
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
            var model = await _serviceDep.GetDepartmentByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            await _serviceDep.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
