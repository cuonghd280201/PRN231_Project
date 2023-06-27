﻿using System.ComponentModel.DataAnnotations;

namespace Bussiness.Dtos.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool IsDoctor { get; set; }
        public bool IsStaff { get; set; }
    }
}
