using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UploadPfpFormDto
    {
        public string Username { get; set; } = null!;
        public IFormFile ProfilePicture { get; set; } = null!;

        internal void Deconstruct(out string username, out IFormFile profilePicture)
        {
            username = Username;
            profilePicture = ProfilePicture;
        }
    }
}