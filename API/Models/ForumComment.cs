using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ForumComment
    {
        [Key]
        public int Id { get; set; }
        public ForumThread Thread { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime TimePosted { get; set; }
        public bool Edited { get; set; }
    }
}