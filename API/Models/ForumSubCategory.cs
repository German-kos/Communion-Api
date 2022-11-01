using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ForumSubCategory
    {
        [Key]
        public int Id { get; set; }
        public ForumCategory Category { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<ForumThread> Threads { get; set; }
    }
}