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

        [Required(ErrorMessage = "Employee is required")]
        [Display(Name = "Employee")]
        public int? EmployeeID { get; set; }

        [Required(ErrorMessage = "Client is required")]
        [Display(Name = "Client")]
        public int? ClientProfileID { get; set; }

        // Internal mapping ID (EmployeeClient table)
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

        [Display(Name = "Employee")]
        public string? EmployeeName { get; set; }

        [Display(Name = "Client")]
        public string? ClientName { get; set; }
    }

    public class AppointmentListViewModel : BaseViewModel
    {
        public List<AppointmentViewModel> Appointments { get; set; } = new();
        
    }
}