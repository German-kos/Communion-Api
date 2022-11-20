using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class DeleteCategoryDto
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; } = null!;

        internal void Deconstruct(out int id, out string name)
        {
            id = categoryId;
            name = categoryName;
        }
    }
}