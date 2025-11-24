using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Services.ViewModels
{
    public class PropertyFeatureViewModel
    {
        public int FeatureID { get; set; }

        [Required]
        [Display(Name = "Feature Name")]
        public string FeatureName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Amenity?")]
        public bool? IsAmenity { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class PropertyFeatureListViewModel : BaseViewModel
    {
        public List<PropertyFeatureViewModel> PropertyFeatures { get; set; } = new();
    }
}
