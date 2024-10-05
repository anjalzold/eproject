using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace eproject.Areas.Admin.Models
{
    public class User : IdentityUser
    {
        public string? Role { get; set; }

        public string? AvatarPath { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }

        public DateTime LastActivityDate { get; set; }


            public string? GoogleId { get; set; }
 


    }
}