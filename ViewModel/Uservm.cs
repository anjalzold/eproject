using System;
using System.Collections.Generic;

namespace eproject.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? Role { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}