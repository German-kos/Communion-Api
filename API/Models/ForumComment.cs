using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace API.Models
{
    public class ForumComment
    {
        [Key]
        public int Id { get; set; }
        public ForumThread Thread { get; set; }
        public int ThreadId { get; set; }
        public AppUser Author { get; set; }
        public string Content { get; set; }
        public DateTime TimePosted { get; set; }
        public bool Edited { get; set; }
        public DateTime? TimeEdited { get; set; }
    }
}