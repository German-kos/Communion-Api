using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public string PublicId { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}