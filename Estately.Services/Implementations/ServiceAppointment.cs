using System.Linq.Expressions;

namespace Estately.Services.Implementations
{
    public class ServiceAppointment : IServiceAppointment
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceAppointment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 1. LIST APPOINTMENTS (SEARCH + PAGINATION)
        public async Task<AppointmentListViewModel> GetAppointmentsPagedAsync(int page, int pageSize, string? search)
        {
            // Step 1: Load all appointments with related data
            var appointments = await _unitOfWork.AppointmentRepository.ReadAllIncluding("Status", "Property", "EmployeeClient");
            var query = appointments.AsQueryable();

            // Step 2: Filtering (case-insensitive search)
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();

                query = query.Where(a =>
                    (a.Notes ?? "").ToLower().Contains(searchLower) ||
                    (a.Property != null && a.Property.Address != null ? a.Property.Address.ToLower().Contains(searchLower) : false) ||
                    (a.Property != null && a.Property.PropertyCode != null ? a.Property.PropertyCode.ToLower().Contains(searchLower) : false) ||
                    (a.Status != null && a.Status.ToString() != null ? a.Status.ToString().ToLower().Contains(searchLower) : false)
                );
            }

            // Step 3: Total count AFTER FILTER
            int totalCount = query.Count();

            // Step 4: Pagination
            var pagedAppointments = query
                .OrderBy(a => a.AppointmentID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Step 5: Build ViewModel
            return new AppointmentListViewModel
            {
                Appointments = pagedAppointments.Select(ConvertToViewModel).ToList(),
                Page = page,
                PageSize = pageSize,
                SearchTerm = search,
                TotalCount = totalCount
            };
        }

        // ====================================================
        // CREATE APPOINTMENT
        // ====================================================
        public async Task CreateAppointmentAsync(AppointmentViewModel model)
        {
            // Resolve or create EmployeeClient mapping based on selected employee and client
            int employeeClientId = await GetOrCreateEmployeeClientIdAsync(model.EmployeeID, model.ClientProfileID);

            var appointment = new TblAppointment
            {
                StatusID = model.StatusID ?? 0,
                PropertyID = model.PropertyID ?? 0,
                EmployeeClientID = employeeClientId,
                AppointmentDate = model.AppointmentDate ?? DateTime.Now,
                Notes = model.Notes ?? string.Empty
            };

            await _unitOfWork.AppointmentRepository.AddAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 2. GET APPOINTMENT BY ID
        // ====================================================
        public async Task<AppointmentViewModel?> GetAppointmentByIdAsync(int id)
        {
            var appointments = await _unitOfWork.AppointmentRepository.ReadAllIncluding("Status", "Property", "EmployeeClient");
            var appointment = appointments.FirstOrDefault(x => x.AppointmentID == id);
            return appointment == null ? null : ConvertToViewModel(appointment);
        }

        // ====================================================
        // 3. UPDATE APPOINTMENT
        // ====================================================
        public async Task UpdateAppointmentAsync(AppointmentViewModel model)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(model.AppointmentID);
            if (appointment == null) return;

            int employeeClientId = await GetOrCreateEmployeeClientIdAsync(model.EmployeeID, model.ClientProfileID);

            appointment.StatusID = model.StatusID ?? 0;
            appointment.PropertyID = model.PropertyID ?? 0;
            appointment.EmployeeClientID = employeeClientId;
            appointment.AppointmentDate = model.AppointmentDate ?? DateTime.Now;
            appointment.Notes = model.Notes ?? string.Empty;

            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 4. DELETE APPOINTMENT
        // ====================================================
        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (appointment == null) return;

            await _unitOfWork.AppointmentRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 5. UPDATE APPOINTMENT STATUS
        // ====================================================
        public async Task UpdateAppointmentStatusAsync(int appointmentId, int statusId)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null) return;

            appointment.StatusID = statusId;

            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.CompleteAsync();
        }

        // ====================================================
        // 6. APPOINTMENT COUNTER (STATS)
        // ====================================================
        public async Task<int> GetAppointmentCounterAsync()
        {
            return await _unitOfWork.AppointmentRepository.CounterAsync();
        }

        // ====================================================
        // 7. GET MAX ID
        // ====================================================
        public int GetMaxIDAsync()
        {
            return _unitOfWork.AppointmentRepository.GetMaxId();
        }

        // ====================================================
        // 8. SEARCH APPOINTMENTS
        // ====================================================
        public async ValueTask<IEnumerable<TblAppointment>> SearchAppointmentAsync(Expression<Func<TblAppointment, bool>> predicate)
        {
            return await _unitOfWork.AppointmentRepository.Search(predicate);
        }

        // ====================================================
        // HELPER: ENTITY -> VIEWMODEL
        // ====================================================
        private AppointmentViewModel ConvertToViewModel(TblAppointment a)
        {
            return new AppointmentViewModel
            {
                AppointmentID = a.AppointmentID,
                StatusID = a.StatusID,
                PropertyID = a.PropertyID,
                EmployeeClientID = a.EmployeeClientID,
                AppointmentDate = a.AppointmentDate,
                Notes = a.Notes,
                StatusName = a.Status?.StatusName ?? a.StatusID.ToString(),
                PropertyName = a.Property?.Address,
                // Map employee/client info if navigation is loaded
                EmployeeID = a.EmployeeClient?.EmployeeID,
                ClientProfileID = a.EmployeeClient?.ClientProfileID,
                EmployeeName = a.EmployeeClient?.Employee != null
                    ? $"{a.EmployeeClient.Employee.FirstName} {a.EmployeeClient.Employee.LastName}"
                    : null,
                ClientName = a.EmployeeClient?.ClientProfile != null
                    ? $"{a.EmployeeClient.ClientProfile.FirstName} {a.EmployeeClient.ClientProfile.LastName}"
                    : null
            };
        }

        private async Task<int> GetOrCreateEmployeeClientIdAsync(int? employeeId, int? clientProfileId)
        {
            if (employeeId == null || clientProfileId == null)
                throw new ArgumentException("EmployeeID and ClientProfileID are required to create an appointment.");

            var allMappings = await _unitOfWork.EmployeeClientRepository.ReadAllAsync();
            var existing = allMappings.FirstOrDefault(ec => ec.EmployeeID == employeeId && ec.ClientProfileID == clientProfileId);

            if (existing != null)
                return existing.EmployeeClientID;

            var newMapping = new TblEmployeeClient
            {
                EmployeeID = employeeId.Value,
                ClientProfileID = clientProfileId.Value,
                AssignmentDate = DateTime.Now,
            };

            await _unitOfWork.EmployeeClientRepository.AddAsync(newMapping);
            await _unitOfWork.CompleteAsync();

            return newMapping.EmployeeClientID;
        }
    }
}