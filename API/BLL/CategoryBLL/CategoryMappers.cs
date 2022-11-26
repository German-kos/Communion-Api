using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;

namespace API.BLL.CategoryBLL
{
    public class CategoryMappers : ICategoryMappers
    {
        public ForumCategoryDto CategoryMapper(Category category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            // Deconstruction
            var (id, name, info, banner, subCategories) = category;

            // Return remap
            return new ForumCategoryDto
            {
                Id = id,
                Name = name,
                Info = info,
                Banner = banner,
                SubCategories = SubCategoryMapper(subCategories),
            };
        }
    }
}