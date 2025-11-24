using System.ComponentModel.DataAnnotations;

namespace Estately.WebApp.ViewModels.Clients
{
    public class ClientProfileViewModel
    {
        [Display(Name = "Client Profile ID")]
        public int ClientProfileID { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(255, ErrorMessage = "First name cannot exceed 255 characters.")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(255, ErrorMessage = "Last name cannot exceed 255 characters.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [StringLength(50, ErrorMessage = "Phone number cannot exceed 50 characters.")]
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [StringLength(800, ErrorMessage = "Profile photo path cannot exceed 800 characters.")]
        [Display(Name = "Profile Photo URL")]
        public string? ProfilePhoto { get; set; }

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; }
    }
}
