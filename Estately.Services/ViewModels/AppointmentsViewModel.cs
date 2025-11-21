using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class AppointmentViewModel
    {
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public int? StatusID { get; set; }

        [Required(ErrorMessage = "Property is required")]
        [Display(Name = "Property")]
        public int? PropertyID { get; set; }

        [Required(ErrorMessage = "Employee/Client is required")]
        [Display(Name = "Employee/Client")]
        public int? EmployeeClientID { get; set; }

        [Required(ErrorMessage = "Appointment date is required")]
        [Display(Name = "Appointment Date")]
        public DateTime? AppointmentDate { get; set; }

        [StringLength(1000)]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Status")]
        public string? StatusName { get; set; }

        [Display(Name = "Property")]
        public string? PropertyName { get; set; }

        [Display(Name = "Employee/Client")]
        public string? EmployeeClientName { get; set; }
    }

    public class AppointmentListViewModel : BaseViewModel
    {
        public List<AppointmentViewModel> Appointments { get; set; } = new();
        
    }
}