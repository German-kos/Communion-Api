using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.DTOs
{
    public class ForumSubCategoryDto
    {
        public int? Id { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<ForumThread>? Threads { get; set; }
    }
}