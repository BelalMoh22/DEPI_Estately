using Estately.Core.Entities;
using Estately.Core.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Estately.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Dashboard()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var userTypeId = User.FindFirst("UserTypeId")?.Value;
            if (userTypeId != "4")
            {
                return RedirectToAction("Index", "App");
            }

            var stats = new
            {
                TotalUsers = (await _unitOfWork.UserRepository.GetAllAsync()).Count(),
                TotalProperties = (await _unitOfWork.PropertyRepository.ReadAllAsync()).Count(),
                TotalAppointments = (await _unitOfWork.AppointmentRepository.ReadAllAsync()).Count(),
                TotalEmployees = (await _unitOfWork.EmployeeRepository.ReadAllAsync()).Count(),
                TotalClients = (await _unitOfWork.ClientProfileRepository.ReadAllAsync()).Count(),
                TotalBranches = (await _unitOfWork.BranchRepository.ReadAllAsync()).Count()
            };

            ViewBag.Stats = stats;
            return View();
        }
    }
}