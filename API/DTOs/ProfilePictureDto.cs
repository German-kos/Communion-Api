using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProfilePictureDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; } = null!;
        public string PublicId { get; set; } = null!;
    }
}