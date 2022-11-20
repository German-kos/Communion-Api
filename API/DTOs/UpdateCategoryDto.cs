using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.DTOs
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? NewCategoryName { get; set; }
        public string? NewInfo { get; set; }
        public IFormFile? NewImageFile { get; set; }

        internal void Deconstruct(out int id, out string name, out string? newName, out string? newInfo, out IFormFile? newImageFile)
        {
            id = CategoryId;
            name = CategoryName;
            newName = NewCategoryName;
            newInfo = NewInfo;
            newImageFile = NewImageFile;
        }
    }
}