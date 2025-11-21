using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Estately.Services.ViewModels;

namespace Estately.Services.Interfaces
{
    public interface IServiceAppointment
    {
        Task<AppointmentListViewModel> GetAppointmentsPagedAsync(int page, int pageSize, string? search);
        Task<AppointmentViewModel?> GetAppointmentByIdAsync(int id);
        Task CreateAppointmentAsync(AppointmentViewModel model); // ADD THIS METHOD
        Task UpdateAppointmentAsync(AppointmentViewModel model);
        Task DeleteAppointmentAsync(int id);
        Task UpdateAppointmentStatusAsync(int appointmentId, int statusId);
        Task<int> GetAppointmentCounterAsync();
        int GetMaxIDAsync();
        ValueTask<IEnumerable<TblAppointment>> SearchAppointmentAsync(Expression<Func<TblAppointment, bool>> predicate);
    }
}