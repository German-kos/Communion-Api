using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AutoSignInFormDto
    {
        public string Username { get; set; } = null!;
        public bool Remember { get; set; } = false;

        internal void Deconstruct(out string username, out bool remember)
        {
            username = Username;
            remember = Remember;
        }
    }
}