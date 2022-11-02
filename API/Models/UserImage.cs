using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace API.Models
{
    public class UserImage
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}