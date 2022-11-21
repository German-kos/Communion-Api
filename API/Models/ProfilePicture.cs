using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace API.Models
{
    public class ProfilePicture
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string PublicId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public int UserId { get; set; }
    }
}