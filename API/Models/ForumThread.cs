using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ForumThread
    {
        [Key]
        public int Id { get; set; }
        public ForumCategory Category { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Views { get; set; }
        public int Comments { get; set; }
        public DateTime TimePosted { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}