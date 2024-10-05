using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using eproject.Areas.Admin.Models;
using eproject.Data;
using eproject.Areas.Admin.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using eproject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;


namespace eproject.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize]

// [Authorize(Roles = "Admin")] // This restricts access to only Admin role


public class UserController : Controller

{

    private readonly AppDbContext _context;

    private readonly IUser _userRepository;

    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;


    public UserController(AppDbContext context, IUser userRepository, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager, SignInManager<User> signInManager)

    {

        _context = context;

        _userManager = userManager;

        _signInManager = signInManager;

        _userRepository = userRepository;

        _webHostEnvironment = webHostEnvironment;

    }


    public async Task<IActionResult> Index()

    {

        var users = await _userRepository.GetAllAsync();


        var userViewModels = users.Select(user => new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            AvatarPath = user.AvatarPath,
            CreatedAt = user.CreatedAt,
            Role = user.Role,
            Roles = new List<string> { user.Role } // Adjust based on your requirements
        }).ToList();




        return View(userViewModels);
    }

    public IActionResult Create()

    {

        return View();

    }


    [HttpPost]

    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(User user, IFormFile avatar)

    {

        if (ModelState.IsValid)

        {

            if (avatar != null && avatar.Length > 0)

            {

                var uniqueFileName = GetUniqueFileName(avatar.FileName);

                var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");

                var filePath = Path.Combine(uploads, uniqueFileName);

                Directory.CreateDirectory(uploads);

                await avatar.CopyToAsync(new FileStream(filePath, FileMode.Create));

                user.AvatarPath = "/images/avatars/" + uniqueFileName;

            }


            _userRepository.Add(user);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _context.SaveChangesAsync(currentUserId);

            return RedirectToAction("Index", "Home", new { area = "Admin" });

        }

        return View(user);

    }

    private string GetUniqueFileName(string fileName)

    {

        fileName = Path.GetFileName(fileName);

        return Path.GetFileNameWithoutExtension(fileName)

               + "_"

               + Guid.NewGuid().ToString().Substring(0, 4)

               + Path.GetExtension(fileName);

    }




    // public IActionResult Privacy()
    // {
    //     return View();
    // }
    //
    // public IActionResult AddUser()
    // {
    //
    //     return View();
    // }

    public async Task<IActionResult> UpdateUser(string id)

    {

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)

        {

            return NotFound();

        }

        return View(user);  // Cast to object to use the correct View() overload
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUser(string id, [Bind("Id,UserName,Email,Role,Bio")] User user, IFormFile? avatar, string? newPassword)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Update only the properties that are allowed to be changed
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;
                existingUser.Bio = user.Bio;

                if (avatar != null && avatar.Length > 0)
                {
                    var uniqueFileName = GetUniqueFileName(avatar.FileName);
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    Directory.CreateDirectory(uploads);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatar.CopyToAsync(fileStream);
                    }
                    existingUser.AvatarPath = "/images/avatars/" + uniqueFileName;
                }

                if (!string.IsNullOrEmpty(newPassword))
                {
                    var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                    var token = await userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var result = await userManager.ResetPasswordAsync(existingUser, token, newPassword);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(existingUser);
                    }
                }

                _userRepository.Update(existingUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(user);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            _userRepository.Remove(user);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _context.SaveChangesAsync(currentUserId);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "An error occurred while deleting user with ID: {UserId}", id);
            return Json(new { success = false, message = "An error occurred while deleting the user." });
        }
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

        return RedirectToAction("Index", "Home", new { area = "" });
    }


    private async Task<bool> UserExists(string id)

    {

        return await _userRepository.GetByIdAsync(id) != null;

    }

    





}
