namespace Estately.Services.ViewModels
{
    [Keyless] // Indicate that this class does not have a primary key
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false; // This property Will help to remember the user login session 
    }
}
