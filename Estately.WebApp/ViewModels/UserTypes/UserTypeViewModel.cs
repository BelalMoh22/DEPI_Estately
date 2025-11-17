using System.ComponentModel.DataAnnotations;

namespace Estately.WebApp.ViewModels.UserTypes
{
    public class UserTypeViewModel
    {
        [Display(Name = "User Type ID")]
        public int UserTypeID { get; set; }

        [Required(ErrorMessage = "User type name is required.")]
        [StringLength(255, ErrorMessage = "User type name cannot exceed 255 characters.")]
        [Display(Name = "User Type Name")]
        public string UserTypeName { get; set; } = string.Empty;
    }
}

