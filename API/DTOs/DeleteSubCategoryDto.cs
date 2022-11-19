using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class DeleteSubCategoryDto
    {
        public string CategoryName { get; set; } = null!;
        public string SubCategoryName { get; set; } = null!;
    }
}