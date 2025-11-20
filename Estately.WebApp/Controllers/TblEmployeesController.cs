using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace Estately.WebApp.Controllers
{
    public class TblEmployeesController : Controller
    {
        private readonly IServiceEmployee _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public TblEmployeesController(IServiceEmployee service, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _service = service;
            _unitOfWork = unitOfWork;
            _env = env;
        }

        // -----------------------------------------------------------
        // INDEX
        // -----------------------------------------------------------
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var vm = await _service.GetEmployeePagedAsync(page, pageSize, search);
            return View(vm);
        }

        // -----------------------------------------------------------
        // EDIT GET
        // -----------------------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetEmployeeByIdAsync(id);
            if (vm == null) return NotFound();

            vm = await BuildEmployeeVMAsync(vm);
            return View(vm);
        }

        // -----------------------------------------------------------
        // EDIT POST
        // -----------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeesViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await BuildEmployeeVMAsync(vm);
                return View(vm);
            }

            await _service.UpdateEmployeeAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        // -----------------------------------------------------------
        // DETAILS
        // -----------------------------------------------------------
        public async Task<IActionResult> Details(int id)
        {
            var vm = await _service.GetEmployeeByIdAsync(id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        // -----------------------------------------------------------
        // DELETE
        // -----------------------------------------------------------
        public async Task<IActionResult> Delete(int id)
        {
            var vm = await _service.GetEmployeeByIdAsync(id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // -----------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------

        private async Task<EmployeesViewModel> BuildEmployeeVMAsync(EmployeesViewModel vm)
        {
            // Job Titles
            var titles = await _unitOfWork.JobTitleRepository.ReadAllAsync();
            vm.JobTitles = titles.Select(j => new JobTitleViewModel
            {
                JobTitleId = j.JobTitleId,
                JobTitleName = j.JobTitleName
            });

            // Branch
            var branches = await _unitOfWork.BranchRepository.ReadAllAsync();
            vm.Branches = branches.Select(b => new BranchViewModel
            {
                BranchID = b.BranchID,
                BranchName = b.BranchName
            });

            // Department
            var departments = await _unitOfWork.DepartmentRepository.ReadAllAsync();
            vm.Departments = departments.Select(d => new DepartmentsViewModel
            {
                DepartmentID = d.DepartmentID,
                DepartmentName = d.DepartmentName
            });

            // Manager list
            var employees = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            vm.Managers = employees.Select(e => new EmployeeSelectViewModel
            {
                EmployeeID = e.EmployeeID,
                FullName = $"{e.FirstName} {e.LastName}"
            });

            // Users list (for linking employee to a user account)
            var users = await _unitOfWork.UserRepository.ReadAllAsync();
            vm.Users = users.Select(u => new UserSelectViewModel
            {
                UserID = u.UserID,
                Email = u.Email
            });

            return vm;
        }
    }
}
