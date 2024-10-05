using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace eproject.ViewModels
{
    public class UserSettingsViewModel
    {
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Display(Name = "Change Avatar")]
        public IFormFile? Avatar { get; set; }

        public string? CurrentAvatarPath { get; set; }

        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
