using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProfileInformationDto
    {
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public string? country { get; set; }
    }
}