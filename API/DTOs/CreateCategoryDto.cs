using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = null!;
        public string Info { get; set; } = null!;
        public IFormFile ImageFile { get; set; } = null!;
    }
}