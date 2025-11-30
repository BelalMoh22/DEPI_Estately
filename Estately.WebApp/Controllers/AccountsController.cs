//using Estately.Core.Entities.Identity;
//using Estately.Services.ViewModels;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;

//namespace Estately.WebApp.Controllers
//{
//    public class AccountsController : Controller
//    {
//        private readonly AccountService _service;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public AccountsController(AccountService service, UserManager<ApplicationUser> userManager)
//        {
//            _service = service;
//            _userManager = userManager;
//        }

//        public IActionResult Register() => View();

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> Register(RegisterUserViewModel model)
//        {
//            if (!ModelState.IsValid) return View(model);
//            int defaultUserType = 1; // Example default user type
//            var (result, newUser, token) = await _service.RegisterAsync(model, defaultUserType);

//            if (!result.Succeeded)
//            {
//                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
//                return View(model);
//            }

//            var confirmationLink = Url.Action(
//                nameof(ConfirmEmail),
//                "Accounts",
//                new { userId = newUser!.Id, token = WebUtility.UrlEncode(token) },
//                protocol: Request.Scheme);

//            await _service.SendEmailConfirmationAsync(newUser.Email, confirmationLink!);

//            TempData["Message"] = "Confirmation email sent. Please check your inbox.";
//            return RedirectToAction(nameof(Login));
//        }

//        public IActionResult Login() => View();

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            if (!ModelState.IsValid) return View(model);

//            var result = await _service.LoginAsync(model);

//            if (result.Succeeded) return RedirectToAction("Index", "App");

//            if (result.IsNotAllowed)
//            {
//                ModelState.AddModelError("", "Please confirm your email before logging in.");
//                return View(model);
//            }
//            ModelState.AddModelError("", "Invalid username or password.");
//            return View(model);
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> Logout()
//        {
//            await _service.LogOutAsync();
//            return RedirectToAction(nameof(Login));
//        }

//        public IActionResult ForgotPassword() => View();

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
//        {
//            if (!ModelState.IsValid) return View(model);

//            var user = await _userManager.FindByEmailAsync(model.Email);
//            if (user == null) return RedirectToAction("ForgotPasswordConfirmation");

//            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
//            var resetUrl = Url.Action("ResetPassword", "Accounts",
//                new { userId = user.Id, token = WebUtility.UrlEncode(token) }, protocol: Request.Scheme);

//            await _service.SendPasswordResetEmailAsync(model.Email, resetUrl!);
//            return RedirectToAction("ForgotPasswordConfirmation");
//        }

//        public IActionResult ForgotPasswordConfirmation() => View();

//        public IActionResult ResetPassword(string userId, string token)
//        {
//            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token)) return BadRequest();
//            var vm = new ResetPasswordViewModel { UserId = userId, Token = token };
//            return View(vm);
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
//        {
//            if (!ModelState.IsValid) return View(model);
//            var result = await _service.ResetPasswordAsync(model.UserId, WebUtility.UrlDecode(model.Token), model.NewPassword);
//            if (!result.Succeeded)
//            {
//                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
//                return View(model);
//            }
//            return RedirectToAction("ResetPasswordConfirmation");
//        }

//        public IActionResult ResetPasswordConfirmation() => View();

//        public async Task<IActionResult> ConfirmEmail(string userId, string token)
//        {
//            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token)) return BadRequest();

//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null) return NotFound();

//            var decodedToken = WebUtility.UrlDecode(token);
//            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

//            if (result.Succeeded) return View("ConfirmEmailSuccess");
//            return View("ConfirmEmailFailed");
//        }
//    }
//}

using Estately.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
namespace Estately.WebApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AccountService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(AccountService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        // ============================
        // REGISTER
        // ============================
        public IActionResult Register() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int defaultUserTypeId = 1;

            var (result, newUser) = await _service.RegisterAsync(model, defaultUserTypeId);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            // Automatically sign in after register with persistent cookie
            var signIn = await _service.SignInAsync(newUser!, true);

            if (!signIn.Succeeded)
            {
                foreach (var error in signIn.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }

        // ============================
        // LOGIN
        // ============================
        public IActionResult Login() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _service.LoginAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "App");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        // ============================
        // LOGOUT
        // ============================
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _service.LogOutAsync();
            return RedirectToAction("Index", "App");
        }
    }
}

