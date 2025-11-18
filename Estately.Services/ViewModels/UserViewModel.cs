using System.ComponentModel.DataAnnotations;

namespace Estately.Services.ViewModels
{
    public class UserViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "User Type is required")]
        [Display(Name = "User Type")]
        public int? UserTypeID { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(255)]
        public string Username { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Password")]
        public string? PasswordHash { get; set; }

        [Display(Name = "Is Employee")]
        public bool? IsEmployee { get; set; }

        [Display(Name = "Is Client")]
        public bool? IsClient { get; set; }

        [Display(Name = "Is Developer")]
        public bool? IsDeveloper { get; set; }

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Is Deleted")]
        public bool? IsDeleted { get; set; } = false;

        [Display(Name = "User Type")]
        public string? UserTypeName { get; set; }
    }

    public class UserListViewModel : BaseViewModel
    {
        public List<UserViewModel> Users { get; set; } = new();
        public List<UserTypeViewModel> UserTypes { get; set; } = new();
    }
}