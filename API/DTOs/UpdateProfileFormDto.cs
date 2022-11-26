using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UpdateProfileFormDto
    {
        public string Username { get; set; } = null!;
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string? Gender { get; set; }
        public string? Bio { get; set; }

        internal void Deconstruct(out string username, out string? dateOfBirth, out string? country, out string? gender, out string? bio)
        {
            username = Username;
            dateOfBirth = DateOfBirth;
            country = Country;
            gender = Gender;
            bio = Bio;
        }

        internal void Deconstruct(out string? dateOfBirth, out string? gender, out string? country, out string? bio)
        {
            dateOfBirth = DateOfBirth;
            country = Country;
            gender = Gender;
            bio = Bio;
        }
    }
}