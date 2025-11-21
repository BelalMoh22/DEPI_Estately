using Microsoft.AspNetCore.Mvc.Rendering;
using Estately.Services.Interfaces;
using Estately.Services.ViewModels;

namespace Estately.WebApp.Controllers
{
    public class TblAppointmentsController : Controller
    {
        private readonly IServiceAppointment _serviceAppointment;
        private readonly IUnitOfWork _unitOfWork;

        public TblAppointmentsController(IServiceAppointment serviceAppointment, IUnitOfWork unitOfWork)
        {
            _serviceAppointment = serviceAppointment;
            _unitOfWork = unitOfWork;
        }

        // =======================================================
        // INDEX (LIST + SEARCH + PAGINATION)
        // =======================================================
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _serviceAppointment.GetAppointmentsPagedAsync(page, pageSize, search);
            return View(model);
        }

        // =======================================================
        // CREATE
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            // Validate that required IDs are selected
            if (model.EmployeeClientID == null || model.PropertyID == null || model.StatusID == null)
            {
                ModelState.AddModelError("", "Please select valid Employee/Client, Property, and Status.");
                await LoadDropdowns();
                return View(model);
            }

            await _serviceAppointment.CreateAppointmentAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // DETAILS (VIEW APPOINTMENT INFO)
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _serviceAppointment.GetAppointmentByIdAsync(id);

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
            var model = await _serviceAppointment.GetAppointmentByIdAsync(id);
            if (model == null)
                return NotFound();

            await LoadDropdowns();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            // check existence using the existing service method
            var existing = await _serviceAppointment.GetAppointmentByIdAsync(model.AppointmentID);
            if (existing == null)
                return NotFound();

            await _serviceAppointment.UpdateAppointmentAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // DELETE
        // =======================================================
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _serviceAppointment.GetAppointmentByIdAsync(id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceAppointment.DeleteAppointmentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // UPDATE APPOINTMENT STATUS
        // =======================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int appointmentId, int statusId)
        {
            await _serviceAppointment.UpdateAppointmentStatusAsync(appointmentId, statusId);
            return RedirectToAction("Edit", new { id = appointmentId });
        }

        // =======================================================
        // PRIVATE HELPER: LOAD DROPDOWNS
        // =======================================================
        private async Task LoadDropdowns()
        {
            // GET ACTUAL DATA FROM DATABASE
            var employeeClients = await _unitOfWork.EmployeeClientRepository.ReadAllAsync();
            var properties = await _unitOfWork.PropertyRepository.ReadAllAsync();
            var statuses = await _unitOfWork.AppointmentStatusRepository.ReadAllAsync();

            // POPULATE DROPDOWNS WITH REAL DATA
            ViewBag.EmployeeClients = new SelectList(employeeClients, "EmployeeClientID", "EmployeeClientID");
            ViewBag.Properties = new SelectList(properties, "PropertyID", "Address");
            ViewBag.Statuses = new SelectList(statuses, "StatusID", "StatusName");
        }
    }
}