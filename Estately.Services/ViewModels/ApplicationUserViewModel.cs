using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class ApplicationUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "User Type")]
        [Required(ErrorMessage = "User Type is required")]
        public int? UserTypeID { get; set; }

        [Display(Name = "User Type")]
        public string? UserTypeName { get; set; }

        // Dropdown list for user types
        public IEnumerable<UserTypeViewModel> UserTypes { get; set; } = new List<UserTypeViewModel>();
    }

    public class ApplicationUserListViewModel : BaseViewModel
    {
        public List<ApplicationUserViewModel> Users { get; set; } = new();
    }
}
