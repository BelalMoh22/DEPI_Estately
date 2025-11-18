using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class CityViewModel
    {
        public int CityID { get; set; }

        [Required(ErrorMessage = "City Name is required")]
        [Display(Name = "City Name")]
        [StringLength(255, ErrorMessage = "City Name cannot exceed 255 characters")]
        public string CityName { get; set; } = string.Empty;

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Is Deleted")]
        public bool? IsDeleted { get; set; } = false;

        [Display(Name = "Zone Count")]
        public int ZoneCount { get; set; }
    }

    public class CityListViewModel : BaseViewModel
    {
        public List<CityViewModel> Cities { get; set; } = new();
        public List<ZoneViewModel> Zones { get; set; } = new();
    }
}