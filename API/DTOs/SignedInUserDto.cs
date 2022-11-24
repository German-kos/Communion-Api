using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class SignedInUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public bool Remember { get; set; } = false;
    }
}