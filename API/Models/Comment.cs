using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public Post Post { get; set; } = null!;
        public int PostId { get; set; }
        public AppUser Author { get; set; } = null!;
        public int AuthorId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime TimePosted { get; set; }
        public bool Modified { get; set; } = false;
        public DateTime? TimeModified { get; set; }
    }
}