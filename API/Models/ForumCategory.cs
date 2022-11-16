using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Models
{
    public class ForumCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Info { get; set; } = null!;
        public ICollection<ForumImage> Banner { get; set; } = null!;
        public ICollection<ForumSubCategory> SubCategories { get; set; } = new List<ForumSubCategory>();

    }
}