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
        public string Name { get; set; }
        public string Info { get; set; }
        public ICollection<ForumImage> Banner { get; set; }
        public ICollection<ForumSubCategory> SubCategories { get; set; }

        public static explicit operator ForumCategory(ActionResult<List<ForumCategory>> v)
        {
            throw new NotImplementedException();
        }
    }
}