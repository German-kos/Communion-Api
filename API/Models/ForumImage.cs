using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ForumImage
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }

        public ForumCategory Category { get; set; }
        public int CategoryId { get; set; }
    }
}