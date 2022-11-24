using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

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

        internal void Deconstruct(out int id, out int userId, out string publicId, out string url)
        {
            id = Id;
            userId = UserId;
            publicId = PublicId;
            url = Url;
        }
    }
}