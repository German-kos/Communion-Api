using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.DTOs
{
    public class UpdateCategoryDto
    {
        public string CategoryName { get; set; } = null!;
        public string? NewCategoryName { get; set; }
        public string? Info { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}