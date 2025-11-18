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

        // ============================================
        // DASHBOARD
        // ============================================
        public async Task<IActionResult> Dashboard()
        {
            var stats = new
            {
                TotalUsers = (await _unitOfWork.UserRepository.ReadAllAsync()).Count(),
                TotalProperties = (await _unitOfWork.PropertyRepository.ReadAllAsync()).Count(),
                TotalAppointments = (await _unitOfWork.AppointmentRepository.ReadAllAsync()).Count(),
                TotalEmployees = (await _unitOfWork.EmployeeRepository.ReadAllAsync()).Count(),
                TotalClients = (await _unitOfWork.ClientProfileRepository.ReadAllAsync()).Count(),
                TotalBranches = (await _unitOfWork.BranchRepository.ReadAllAsync()).Count()
            };

            ViewBag.Stats = stats;
            return View();
        }

        #region Generic CRUD Helper Methods

        // Generic methods for simpler entities (Cities, Zones, Branches, Departments, etc.)
        // These follow the same pattern but are more concise

        #endregion

        #region Appointments CRUD

        [HttpGet]
        public async Task<IActionResult> Appointments(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var allAppointments = await _unitOfWork.AppointmentRepository.ReadAllIncluding("Property", "Status", "EmployeeClient.Employee", "EmployeeClient.ClientProfile");
            var query = allAppointments.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(a => (a.Property != null && a.Property.Address.Contains(searchTerm)) || (a.Notes != null && a.Notes.Contains(searchTerm)));

            var totalCount = query.Count();
            var appointments = query.OrderByDescending(a => a.AppointmentDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Appointments = appointments.Select(a => new AppointmentViewModel
            {
                AppointmentID = a.AppointmentID,
                StatusID = a.StatusID,
                PropertyID = a.PropertyID,
                EmployeeClientID = a.EmployeeClientID,
                AppointmentDate = a.AppointmentDate,
                Notes = a.Notes,
                StatusName = a.Status?.StatusName,
                PropertyTitle = a.Property?.Address,
                ClientName = a.EmployeeClient?.ClientProfile?.FirstName + " " + a.EmployeeClient?.ClientProfile?.LastName,
                EmployeeName = a.EmployeeClient?.Employee?.FirstName + " " + a.EmployeeClient?.Employee?.LastName
            }).ToList();
            ViewBag.Statuses = new SelectList(await _unitOfWork.AppointmentStatusRepository.ReadAllAsync(), "StatusId", "StatusName");
            ViewBag.Properties = new SelectList(await _unitOfWork.PropertyRepository.ReadAllAsync(), "PropertyID", "Title");
            ViewBag.EmployeeClients = new SelectList(await _unitOfWork.EmployeeClientRepository.ReadAllIncluding("Employee", "ClientProfile"), "EmployeeClientID", "EmployeeClientID");
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AppointmentRepository.AddAsync(new TblAppointment
                {
                    StatusID = model.StatusID,
                    PropertyID = model.PropertyID,
                    EmployeeClientID = model.EmployeeClientID,
                    AppointmentDate = model.AppointmentDate,
                    Notes = model.Notes
                });
                _unitOfWork.CompleteAsync();
            }
            return RedirectToAction("Appointments");
        }

        [HttpPost]
        public async Task<IActionResult> EditAppointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(model.AppointmentID);
                if (appointment != null)
                {
                    appointment.StatusID = model.StatusID;
                    appointment.PropertyID = model.PropertyID;
                    appointment.EmployeeClientID = model.EmployeeClientID;
                    appointment.AppointmentDate = model.AppointmentDate;
                    appointment.Notes = model.Notes;
                    _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("Appointments");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            _unitOfWork.AppointmentRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("Appointments");
        }

        #endregion

        #region Employees CRUD

        [HttpGet]
        public async Task<IActionResult> Employees(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var allEmployees = await _unitOfWork.EmployeeRepository.ReadAllIncluding("BranchDepartment", "JobTitle", "ReportsToNavigation", "User");
            var query = allEmployees.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(e => e.FirstName.Contains(searchTerm) || e.LastName.Contains(searchTerm) || e.Phone.Contains(searchTerm));

            var totalCount = query.Count();
            var employees = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Employees = employees.Select(e => new EmployeeViewModel
            {
                EmployeeID = e.EmployeeID,
                UserID = e.UserID,
                BranchDepartmentId = e.BranchDepartmentId,
                JobTitleId = e.JobTitleId,
                ReportsTo = e.ReportsTo,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Gender = e.Gender,
                Age = e.Age,
                Phone = e.Phone,
                Nationalid = e.Nationalid,
                ProfilePhoto = e.ProfilePhoto,
                Salary = e.Salary,
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                BranchDepartmentName = e.BranchDepartment != null ? $"{e.BranchDepartment.Branch?.BranchName} - {e.BranchDepartment.Department?.DepartmentName}" : null,
                JobTitleName = e.JobTitle?.JobTitleName,
                ReportsToName = e.ReportsToNavigation != null ? $"{e.ReportsToNavigation.FirstName} {e.ReportsToNavigation.LastName}" : null,
                Username = e.User?.Username
            }).ToList();
            ViewBag.BranchDepartments = new SelectList(await _unitOfWork.BranchDepartmentRepository.ReadAllIncluding("Branch", "Department"), "BranchDepartmentID", "BranchDepartmentID");
            ViewBag.JobTitles = new SelectList(await _unitOfWork.JobTitleRepository.ReadAllAsync(), "JobTitleId", "JobTitleName");
            var reportsToEmployees = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            ViewBag.ReportsToEmployees = new SelectList(reportsToEmployees.Select(e => new { e.EmployeeID, FullName = $"{e.FirstName} {e.LastName}" }), "EmployeeID", "FullName");
            ViewBag.Users = new SelectList(await _unitOfWork.UserRepository.ReadAllAsync(), "UserID", "Username");
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            ViewBag.BranchDepartments = new SelectList(await _unitOfWork.BranchDepartmentRepository.ReadAllIncluding("Branch", "Department"), "BranchDepartmentID", "BranchDepartmentID");
            ViewBag.JobTitles = new SelectList(await _unitOfWork.JobTitleRepository.ReadAllAsync(), "JobTitleId", "JobTitleName");
            var reportsToEmployees = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            ViewBag.ReportsToEmployees = new SelectList(reportsToEmployees.Select(e => new { e.EmployeeID, FullName = $"{e.FirstName} {e.LastName}" }), "EmployeeID", "FullName");
            ViewBag.Users = new SelectList(await _unitOfWork.UserRepository.ReadAllAsync(), "UserID", "Username");
            return View(new EmployeeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.EmployeeRepository.AddAsync(new TblEmployee
                {
                    UserID = model.UserID,
                    BranchDepartmentId = model.BranchDepartmentId,
                    JobTitleId = model.JobTitleId,
                    ReportsTo = model.ReportsTo,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    Age = model.Age,
                    Phone = model.Phone,
                    Nationalid = model.Nationalid,
                    ProfilePhoto = model.ProfilePhoto,
                    Salary = model.Salary,
                    HireDate = model.HireDate ?? DateTime.Now,
                    IsActive = true
                });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("Employees");
            }
            ViewBag.BranchDepartments = new SelectList(await _unitOfWork.BranchDepartmentRepository.ReadAllIncluding("Branch", "Department"), "BranchDepartmentID", "BranchDepartmentID");
            ViewBag.JobTitles = new SelectList(await _unitOfWork.JobTitleRepository.ReadAllAsync(), "JobTitleId", "JobTitleName");
            var reportsToEmployees = await _unitOfWork.EmployeeRepository.ReadAllAsync();
            ViewBag.ReportsToEmployees = new SelectList(reportsToEmployees.Select(e => new { e.EmployeeID, FullName = $"{e.FirstName} {e.LastName}" }), "EmployeeID", "FullName");
            ViewBag.Users = new SelectList(await _unitOfWork.UserRepository.ReadAllAsync(), "UserID", "Username");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            var model = new EmployeeViewModel
            {
                EmployeeID = employee.EmployeeID,
                UserID = employee.UserID,
                BranchDepartmentId = employee.BranchDepartmentId,
                JobTitleId = employee.JobTitleId,
                ReportsTo = employee.ReportsTo,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Age = employee.Age,
                Phone = employee.Phone,
                Nationalid = employee.Nationalid,
                ProfilePhoto = employee.ProfilePhoto,
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive
            };
            ViewBag.BranchDepartments = new SelectList(await _unitOfWork.BranchDepartmentRepository.ReadAllIncluding("Branch", "Department"), "BranchDepartmentID", "BranchDepartmentID", employee.BranchDepartmentId);
            ViewBag.JobTitles = new SelectList(await _unitOfWork.JobTitleRepository.ReadAllAsync(), "JobTitleId", "JobTitleName", employee.JobTitleId);
            ViewBag.ReportsToEmployees = new SelectList(await _unitOfWork.EmployeeRepository.ReadAllAsync(), "EmployeeID", "FirstName", employee.ReportsTo);
            ViewBag.Users = new SelectList(await _unitOfWork.UserRepository.ReadAllAsync(), "UserID", "Username", employee.UserID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(model.EmployeeID);
                if (employee != null)
                {
                    employee.UserID = model.UserID;
                    employee.BranchDepartmentId = model.BranchDepartmentId;
                    employee.JobTitleId = model.JobTitleId;
                    employee.ReportsTo = model.ReportsTo;
                    employee.FirstName = model.FirstName;
                    employee.LastName = model.LastName;
                    employee.Gender = model.Gender;
                    employee.Age = model.Age;
                    employee.Phone = model.Phone;
                    employee.Nationalid = model.Nationalid;
                    employee.ProfilePhoto = model.ProfilePhoto;
                    employee.Salary = model.Salary;
                    employee.HireDate = model.HireDate;
                    employee.IsActive = model.IsActive;
                    _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("Employees");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee != null)
            {
                employee.IsActive = false;
                _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                _unitOfWork.CompleteAsync();
            }
            return RedirectToAction("Employees");
        }

        #endregion

        #region Lookup Tables CRUD (Property Types, Statuses, etc.)

        [HttpGet]
        public async Task<IActionResult> PropertyTypes()
        {
            ViewBag.PropertyTypes = (await _unitOfWork.PropertyTypeRepository.ReadAllAsync())
                .Select(pt => new LkpPropertyTypeViewModel { PropertyTypeID = pt.PropertyTypeID, TypeName = pt.TypeName }).ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CreatePropertyType()
        {
            return View(new LkpPropertyTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePropertyType(LkpPropertyTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PropertyTypeRepository.AddAsync(new LkpPropertyType { TypeName = model.TypeName });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("PropertyTypes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPropertyType(int id)
        {
            var pt = await _unitOfWork.PropertyTypeRepository.GetByIdAsync(id);
            if (pt == null) return NotFound();
            var model = new LkpPropertyTypeViewModel { PropertyTypeID = pt.PropertyTypeID, TypeName = pt.TypeName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPropertyType(LkpPropertyTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pt = await _unitOfWork.PropertyTypeRepository.GetByIdAsync(model.PropertyTypeID);
                if (pt != null)
                {
                    pt.TypeName = model.TypeName;
                    _unitOfWork.PropertyTypeRepository.UpdateAsync(pt);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("PropertyTypes");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePropertyType(int id)
        {
            _unitOfWork.PropertyTypeRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("PropertyTypes");
        }

        [HttpGet]
        public async Task<IActionResult> PropertyStatuses()
        {
            ViewBag.PropertyStatuses = (await _unitOfWork.PropertyStatusRepository.ReadAllAsync())
                .Select(ps => new LkpPropertyStatusViewModel { StatusID = ps.StatusID, StatusName = ps.StatusName, Description = ps.Description }).ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CreatePropertyStatus()
        {
            return View(new LkpPropertyStatusViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePropertyStatus(LkpPropertyStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PropertyStatusRepository.AddAsync(new LkpPropertyStatus { StatusName = model.StatusName, Description = model.Description });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("PropertyStatuses");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPropertyStatus(int id)
        {
            var ps = await _unitOfWork.PropertyStatusRepository.GetByIdAsync(id);
            if (ps == null) return NotFound();
            var model = new LkpPropertyStatusViewModel { StatusID = ps.StatusID, StatusName = ps.StatusName, Description = ps.Description };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPropertyStatus(LkpPropertyStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ps = await _unitOfWork.PropertyStatusRepository.GetByIdAsync(model.StatusID);
                if (ps != null)
                {
                    ps.StatusName = model.StatusName;
                    ps.Description = model.Description;
                    _unitOfWork.PropertyStatusRepository.UpdateAsync(ps);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("PropertyStatuses");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePropertyStatus(int id)
        {
            _unitOfWork.PropertyStatusRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("PropertyStatuses");
        }

        [HttpGet]
        public async Task<IActionResult> UserTypes()
        {
            ViewBag.UserTypes = (await _unitOfWork.UserTypeRepository.ReadAllAsync())
                .Select(ut => new UserTypeViewModel { UserTypeID = ut.UserTypeID, UserTypeName = ut.UserTypeName, Description = ut.Description }).ToList();
            return View();
        }

        [HttpGet]
        public IActionResult CreateUserType()
        {
            return View(new UserTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserType(UserTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.UserTypeRepository.AddAsync(new LkpUserType { UserTypeName = model.UserTypeName, Description = model.Description });
                _unitOfWork.CompleteAsync();
                return RedirectToAction("UserTypes");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserType(int id)
        {
            var ut = await _unitOfWork.UserTypeRepository.GetByIdAsync(id);
            if (ut == null) return NotFound();
            var model = new UserTypeViewModel { UserTypeID = ut.UserTypeID, UserTypeName = ut.UserTypeName, Description = ut.Description };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserType(UserTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ut = await _unitOfWork.UserTypeRepository.GetByIdAsync(model.UserTypeID);
                if (ut != null)
                {
                    ut.UserTypeName = model.UserTypeName;
                    ut.Description = model.Description;
                    _unitOfWork.UserTypeRepository.UpdateAsync(ut);
                    _unitOfWork.CompleteAsync();
                }
            }
            return RedirectToAction("UserTypes");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            _unitOfWork.UserTypeRepository.DeleteAsync(id);
            _unitOfWork.CompleteAsync();
            return RedirectToAction("UserTypes");
        }

        #endregion
    }
}