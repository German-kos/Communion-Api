using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CreateCategoryDto
    {
        public CreateCategoryDto(string name, string info, IFormFile imageFile)
        {
            Name = name;
            Info = info;
            ImageFile = imageFile;
        }
        public string Name { get; set; }
        public string Info { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}