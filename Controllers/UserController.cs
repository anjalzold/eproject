using Microsoft.AspNetCore.Mvc;
using eproject.ViewModels;
using eproject.Areas.Admin.Models;
using eproject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace eproject.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUser _userRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(AppDbContext context, IUser userRepository,
            IWebHostEnvironment webHostEnvironment, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "User", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Check if the user already exists
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                // User exists, sign them in
                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false, IdentityConstants.ApplicationScheme);
                    return RedirectToLocal(returnUrl);
                }
            }
            else
            {
                // User doesn't exist, create a new account
                user = new User { UserName = email, Email = email };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    createResult = await _userManager.AddLoginAsync(user, info);
                    if (createResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, IdentityConstants.ApplicationScheme);
                        return RedirectToLocal(returnUrl);
                    }
                }
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed
            return View("Login");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        // public IActionResult Index()
        // {
        //     return View();
        // }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model, IFormFile avatar)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Bio = model.Bio,
                    Role = "User"
                };

                if (avatar != null && avatar.Length > 0)
                {
                    var uniqueFileName = GetUniqueFileName(avatar.FileName);
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    Directory.CreateDirectory(uploads);
                    await avatar.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    user.AvatarPath = "/images/avatars/" + uniqueFileName;
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Ensure the "User" role exists before assigning it
                    await EnsureRoleExists("User");
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        private async Task EnsureRoleExists(string roleName)
        {
            var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        user.LastLoginDate = DateTime.Now;
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Sign out of the default authentication scheme
            await _signInManager.SignOutAsync();

            // Sign out of the external authentication scheme
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return RedirectToAction("Index", "Home");
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        [Authorize]
        public async Task<int> GetCurrentUserId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user != null ? int.Parse(user.Id) : 0;
        }

        

        [HttpGet]
        public IActionResult Settings()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new UserSettingsViewModel
            {
                UserName = user.UserName,
                CurrentAvatarPath = user.AvatarPath
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(UserSettingsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {

                // Update Username
                if (user.UserName != model.UserName)
                {
                    user.UserName = model.UserName;
                }

                // Update Avatar
                if (model.Avatar != null)
                {
                    var uniqueFileName = GetUniqueFileName(model.Avatar.FileName);
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    Directory.CreateDirectory(uploads);
                    await model.Avatar.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    user.AvatarPath = "/images/avatars/" + uniqueFileName;
                }

                // Update Password
                if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }
                user.LastActivityDate = DateTime.Now;

                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["StatusMessage"] = "Your settings have been updated";
                    return RedirectToAction("Settings");
                }
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.CurrentAvatarPath = user.AvatarPath;
            return View(model);
        }






    }
}