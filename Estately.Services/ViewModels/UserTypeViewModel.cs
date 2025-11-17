using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class UserTypeViewModel
    {
        public int UserTypeID { get; set; }

        [Required(ErrorMessage = "User Type Name is required")]
        [StringLength(100, ErrorMessage = "User Type Name cannot exceed 100 characters")]
        [Display(Name = "User Type Name")]
        public string UserTypeName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        // Optional: User count for this type
        public int? UserCount { get; set; }
    }

    public class UserTypeListViewModel : BaseViewModel
    {
        public List<UserTypeViewModel> UserTypes { get; set; } = new();
    }
}