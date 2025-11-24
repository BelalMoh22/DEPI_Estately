//using Microsoft.Extensions.Logging;
using System.Security.Claims;
//using Estately.Services.Interfaces.Email;
//using System.Net;

//namespace Estately.Services.Implementations
//{
//    public class AccountService
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly IEmailSender _emailSender;
//        private readonly ILogger<AccountService> _logger;

//        public AccountService(
//            UserManager<ApplicationUser> userManager,
//            SignInManager<ApplicationUser> signInManager,
//            IEmailSender emailSender,
//            ILogger<AccountService> logger)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _emailSender = emailSender;
//            _logger = logger;
//        }

//        // returns (result, user, emailConfirmationToken)
//        public async Task<(IdentityResult Result, ApplicationUser? User, string? Token)> RegisterAsync(RegisterUserViewModel model, int userTypeId)
//        {
//            if (await _userManager.FindByNameAsync(model.UserName) != null)
//                return (IdentityResult.Failed(new IdentityError { Code = "DuplicateUserName", Description = "Username already taken." }), null, null);

//            if (await _userManager.FindByEmailAsync(model.Email) != null)
//                return (IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "Email already registered." }), null, null);

//            var user = new ApplicationUser
//            {
//                UserName = model.UserName,
//                Email = model.Email,
//                CreatedAt = model.CreatedAt ?? DateTime.UtcNow,
//                UserTypeID = userTypeId
//            };

//            var result = await _userManager.CreateAsync(user, model.Password);
//            if (!result.Succeeded) return (result, null, null);

//            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//            return (result, user, token);
//        }

//        // sign in a user instance (returns IdentityResult so we can return errors)
//        public async Task<IdentityResult> SignInAsync(ApplicationUser user, bool isPersistent)
//        {
//            if (user == null) return IdentityResult.Failed(new IdentityError { Code = "UserNull", Description = "User is null." });

//            var found = await _userManager.FindByNameAsync(user.UserName);
//            if (found == null) return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "User not found." });

//            await _signInManager.SignInAsync(found, isPersistent);
//            return IdentityResult.Success;
//        }

//        // login with username + password; blocks login until email confirmed
//        public async Task<SignInResult> LoginAsync(LoginViewModel model)
//        {
//            var user = await _userManager.FindByNameAsync(model.UserName);
//            if (user == null) return SignInResult.Failed;

//            if (!user.EmailConfirmed)
//                return SignInResult.NotAllowed;

//            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
//            return result;
//        }

//        public async Task LogOutAsync()
//        {
//            await _signInManager.SignOutAsync();
//        }

//        public async Task SendPasswordResetEmailAsync(string email, string resetUrl)
//        {
//            var subject = "Reset your password";
//            var html = $@"<p>Click to reset your password:</p><p><a href=""{resetUrl}"">Reset Password</a></p>";
//            await _emailSender.SendEmailAsync(email, subject, html);
//        }

//        public async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
//        {
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null) return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "User not found." });
//            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
//            return result;
//        }

//        public async Task SendEmailConfirmationAsync(string email, string confirmationUrl)
//        {
//            var subject = "Confirm your email";
//            var html = $@"<p>Click to confirm your email:</p><p><a href=""{confirmationUrl}"">Confirm Email</a></p>";
//            await _emailSender.SendEmailAsync(email, subject, html);
//        }
//    }
//}

using Microsoft.Extensions.Logging;

namespace Estately.Services.Implementations
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Register user without Email Confirmation Feature
        public async Task<(IdentityResult Result, ApplicationUser? User)> RegisterAsync(RegisterUserViewModel model, int userTypeId)
        {
            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return (IdentityResult.Failed(new IdentityError { Code = "DuplicateUserName", Description = "Username already taken." }), null);

            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return (IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "Email already registered." }), null);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                CreatedAt = model.CreatedAt ?? DateTime.UtcNow,
                UserTypeID = userTypeId
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            return (result, user);
        }

        // Sign In
        public async Task<IdentityResult> SignInAsync(ApplicationUser user, bool isPersistent)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Code = "UserNull", Description = "User is null." });

            var found = await _userManager.FindByNameAsync(user.UserName);
            if (found == null)
                return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "User not found." });

            await _signInManager.SignInAsync(found, isPersistent);
            return IdentityResult.Success;
        }

        // Login (No email confirmation check)
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return SignInResult.Failed;

            var passwordResult = await _signInManager.CheckPasswordSignInAsync(
                user,
                model.Password,
                lockoutOnFailure: false
            );

            if (!passwordResult.Succeeded)
                return passwordResult;

            var extraClaims = new List<Claim>();

            if (user.UserTypeID.HasValue)
            {
                extraClaims.Add(new Claim("UserTypeId", user.UserTypeID.Value.ToString()));
            }

            await _signInManager.SignInWithClaimsAsync(user, model.RememberMe, extraClaims);

            return SignInResult.Success;
        }

        // Logout
        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
