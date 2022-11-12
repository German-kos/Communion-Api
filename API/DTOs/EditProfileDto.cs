using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class EditProfileDto
    {
        public string DateOfBirth { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
    }
}