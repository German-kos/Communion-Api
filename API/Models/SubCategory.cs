using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public List<Post> Posts { get; set; } = new List<Post>();

        // Deconstructor for SubCategoryMapper
        internal void Deconstruct(out int id, out int categoryId, out string name)
        {
            id = Id;
            categoryId = CategoryId;
            name = Name;
        }
    }
}