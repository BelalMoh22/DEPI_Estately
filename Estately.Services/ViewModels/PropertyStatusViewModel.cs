using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class PropertyStatusViewModel
    {
        public int StatusID { get; set; }

        [Required(ErrorMessage = "Status Name is required")]
        [StringLength(100, ErrorMessage = "Status Name cannot exceed 100 characters")]
        [Display(Name = "Status Name")]
        public string StatusName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Optional: If you want to show related properties count
        [Display(Name = "Properties Count")]
        public int? PropertiesCount { get; set; }
    }

    public class PropertyStatusListViewModel : BaseViewModel
    {
        public List<PropertyStatusViewModel> PropertyStatuses { get; set; } = new();

        // Optional: If you need dropdown data or related entities
        // public List<LkpSomeRelatedViewModel> RelatedData { get; set; } = new();
    }
}