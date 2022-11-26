using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
    }
}