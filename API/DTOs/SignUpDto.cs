using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{

    public class SignUpFormDto
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;

        internal void Deconstruct(out string username, out string password, out string name, out string email)
        {
            username = Username;
            password = Password;
            name = Name;
            email = Email;
        }
    }
}