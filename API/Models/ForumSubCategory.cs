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
        public ForumCategory Category { get; set; } = null!;
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<ForumThread>? Threads { get; set; }

        internal void Deconstruct(out int id, out int categoryId, out string name)
        {
            id = Id;
            categoryId = CategoryId;
            name = Name;
        }
    }
}