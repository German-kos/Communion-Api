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
        public ForumSubCategory SubCategory { get; set; }
        public int SubCategoryId { get; set; }
        public AppUser Author { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int ViewsAmount { get; set; }
        public int CommentsAmount { get; set; }
        public DateTime TimePosted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool Edited { get; set; }
        public DateTime? TimeEdited { get; set; }
        public ICollection<ForumComment> Comments { get; set; }


    }
}