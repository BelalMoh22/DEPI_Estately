using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class PropertyTypeViewModel
    {
        public int PropertyTypeID { get; set; }

        [Required(ErrorMessage = "Type name is required")]
        [Display(Name = "Type Name")]
        [StringLength(255, ErrorMessage = "Type name cannot exceed 255 characters")]
        public string TypeName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }

    public class PropertyTypeListViewModel : BaseViewModel
    {
        public List<PropertyTypeViewModel> PropertyTypes { get; set; } = new();
    }
}