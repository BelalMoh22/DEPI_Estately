using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class PropertyViewModel
    {
        public int PropertyID { get; set; }

        // -----------------------
        // Foreign Keys
        // -----------------------
        [Required(ErrorMessage = "Developer Profile is required")]
        public int DeveloperProfileID { get; set; }

        [Required(ErrorMessage = "Property Type is required")]
        public int PropertyTypeID { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Zone is required")]
        public int ZoneID { get; set; }

        public int? AgentId { get; set; } // Optional

        // -----------------------
        // Property Fields
        // -----------------------
        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Area { get; set; }

        public DateTime? ListingDate { get; set; } = DateTime.Now;

        public decimal? ExpectedRentPrice { get; set; }

        public string FirstImage { get; set; } = "default.jpg";

        public bool? IsDeleted { get; set; } = false; // ignored in UI

        [Required]
        public int YearBuilt { get; set; }

        [Required]
        public int FloorsNo { get; set; }

        [Required]
        public int BedsNo { get; set; }

        [Required]
        public int BathsNo { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        public bool? IsFurnished { get; set; }

        public string PropertyCode { get; set; } = string.Empty; // auto-generated

        // -----------------------
        // Images
        // -----------------------
        public List<PropertyImageViewModel> Images { get; set; } = new();
        public List<IFormFile>? UploadedFiles { get; set; }
        public List<int>? ImagesToDelete { get; set; }

        // -----------------------
        // Features
        // -----------------------
        public List<int> SelectedFeatures { get; set; } = new();
        public List<PropertyFeatureViewModel> AllFeatures { get; set; } = new();
        public List<PropertyFeatureMappingViewModel> FeaturesMappings { get; set; } = new();

        // -----------------------
        // Dropdown Lists
        // -----------------------
        public IEnumerable<LkpPropertyTypeViewModel> PropertyTypes { get; set; } = new List<LkpPropertyTypeViewModel>();
        public IEnumerable<PropertyStatusViewModel> Statuses { get; set; } = new List<PropertyStatusViewModel>();
        public IEnumerable<DeveloperProfileViewModel> Developers { get; set; } = new List<DeveloperProfileViewModel>();
        public IEnumerable<ZonesViewModel> Zones { get; set; } = new List<ZonesViewModel>();
        public IEnumerable<EmployeeViewModel> Agents { get; set; } = new List<EmployeeViewModel>();
        // -----------------------
        // Navigation Names
        public string? CityName { get; set; }
        public string PropertyTypeName { get; set; } = "";
        public string ZoneName { get; set; } = "";
        public string? StatusName { get; set; } = "Available";
        public string? DeveloperTitle { get; set; } = "";
        public string? AgentName { get; set; } = "";

    }

    // -----------------------
    // Images
    // -----------------------
    public class PropertyImageViewModel
    {
        public int ImageID { get; set; }
        public string ImagePath { get; set; }
        public DateTime? UploadedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
    
    // -----------------------
    // Features (NEW)
    // -----------------------
    public class PropertyFeatureViewModel
    {
        public int FeatureID { get; set; }
        public string FeatureName { get; set; }
    }

    public class PropertyFeatureMappingViewModel
    {
        public int PropertyID { get; set; }
        public int FeatureID { get; set; }
    }

    // -----------------------
    // List ViewModel
    // -----------------------
    public class PropertyListViewModel : BaseViewModel
    {
        public List<PropertyViewModel> Properties { get; set; } = new();
        public List<LkpPropertyTypeViewModel> PropertyTypes { get; set; } = new();
        public List<PropertyStatusViewModel> PropertyStatuses { get; set; } = new();
        public List<DeveloperProfileViewModel> DeveloperProfiles { get; set; } = new();
        public List<ZonesViewModel> Zones { get; set; } = new();
        public List<EmployeeViewModel> Agents { get; set; } = new();

        // List of features for filters
        public List<PropertyFeatureViewModel> Features { get; set; } = new();
    }
}