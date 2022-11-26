using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Banner { get; set; }
        public ICollection<ForumSubCategoryDto> SubCategories { get; set; }
    }
}