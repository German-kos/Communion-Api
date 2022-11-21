using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace API.Models
{
    public class ForumThread
    {
        [Key]
        public int Id { get; set; }
        public SubCategory SubCategory { get; set; } = null!;
        public int SubCategoryId { get; set; }
        public AppUser Author { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int ViewsAmount { get; set; }
        public int CommentsAmount { get; set; }
        public DateTime TimePosted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool Modified { get; set; } = false;
        public DateTime? TimeModified { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();

    }
}