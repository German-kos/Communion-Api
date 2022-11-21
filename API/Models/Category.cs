using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Info { get; set; } = null!;
        public Banner Banner { get; set; } = null!;
        public List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();


        internal void Deconstruct(out int id, out string name, out string info, out string banner, out List<SubCategory> subCategories)
        {
            id = Id;
            name = Name;
            info = Info;
            banner = Banner.Url;
            subCategories = SubCategories.ToList<SubCategory>();
        }
    }
}