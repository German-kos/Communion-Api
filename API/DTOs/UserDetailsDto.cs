using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserDetailsDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string interests { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}