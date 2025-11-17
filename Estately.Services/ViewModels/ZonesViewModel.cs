using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.ViewModels
{
    public class ZonesViewModel
    {
        public int ZoneId { get; set; }
        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "Zone Name is required")]
        [StringLength(255)]
        [Display(Name = "Zone Name")]
        public string ZoneName { get; set; } = string.Empty;
        [Display(Name = "City Name")]
        public string? City { get; set; }
        [Display(Name = "Is Deleted")]
        public bool? IsDeleted { get; set; } = false;
    }

    public class ZonesListViewModel : BaseViewModel
    {
        public List<ZonesViewModel> Zones { get; set; } = new();
    }
}
