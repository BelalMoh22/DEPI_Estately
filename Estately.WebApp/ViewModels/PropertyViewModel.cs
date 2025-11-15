using System.ComponentModel.DataAnnotations;

namespace Estately.WebApp.ViewModels
{
    public class PropertyViewModel
    {
        public int PropertyID { get; set; }

        [Required(ErrorMessage = "Developer Profile is required")]
        [Display(Name = "Developer Profile")]
        public int DeveloperProfileID { get; set; }

        [Required(ErrorMessage = "Property Type is required")]
        [Display(Name = "Property Type")]
        public int PropertyTypeID { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Zone is required")]
        [Display(Name = "Zone")]
        public int ZoneID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Area is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Area must be greater than 0")]
        [Display(Name = "Area")]
        public decimal Area { get; set; }

        [Display(Name = "Listing Date")]
        public DateTime ListingDate { get; set; } = DateTime.Now;

        [Display(Name = "Expected Rent Price")]
        public decimal? ExpectedRentPrice { get; set; }

        [Display(Name = "Is Deleted")]
        public bool? IsDeleted { get; set; }

        // Navigation properties for display
        [Display(Name = "Developer Name")]
        public string? DeveloperName { get; set; }

        [Display(Name = "Property Type")]
        public string? PropertyTypeName { get; set; }

        [Display(Name = "Status")]
        public string? StatusName { get; set; }

        [Display(Name = "Zone")]
        public string? ZoneName { get; set; }
    }

    public class PropertyListViewModel : BaseViewModel
    {
        public List<PropertyViewModel> Properties { get; set; } = new();
        public List<LkpPropertyTypeViewModel> PropertyTypes { get; set; } = new();
        public List<LkpPropertyStatusViewModel> PropertyStatuses { get; set; } = new();
        public List<DeveloperProfileViewModel> DeveloperProfiles { get; set; } = new();
        public List<ZoneViewModel> Zones { get; set; } = new();
    }
}

