using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class SignInFormDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Remember { get; set; } = false;

        internal void Deconstruct(out string username, out string password)
        {
            username = Username;
            password = Password;
        }

        internal void Deconstruct(out string username, out string password, out bool remember)
        {
            username = Username;
            password = Password;
            remember = Remember;
        }
    }
}