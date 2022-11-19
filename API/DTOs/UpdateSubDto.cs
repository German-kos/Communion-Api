using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UpdateSubDto
    {
        public string CategoryName { get; set; } = null!;
        public string SubName { get; set; } = null!;
        public string NewSubName { get; set; } = null!;

        internal void Deconstruct(out string categoryName, out string subName, out string newSubName)
        {
            categoryName = CategoryName;
            subName = SubName;
            newSubName = NewSubName;
        }
    }
}