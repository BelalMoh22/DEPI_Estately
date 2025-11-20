using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.ViewModels
{
    public class AppointmentViewModel
    {
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public int StatusID { get; set; }

        [Required(ErrorMessage = "Property is required")]
        [Display(Name = "Property")]
        public int PropertyID { get; set; }

        [Required(ErrorMessage = "Employee Client is required")]
        [Display(Name = "Employee Client")]
        public int EmployeeClientID { get; set; }

        [Required(ErrorMessage = "Appointment Date is required")]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
        public string? Notes { get; set; }

        [Display(Name = "Status Name")]
        public string? StatusName { get; set; }

        [Display(Name = "Property Title")]
        public string? PropertyTitle { get; set; }

        [Display(Name = "Client Name")]
        public string? ClientName { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }
    }
}
