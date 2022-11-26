using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ICategoryMappers
    {
        /// <summary>
        /// Remap <paramref name="Category"/> to <paramref name="CategoryDto"/>.
        /// </summary>
        /// <param name="category">The category to remap.</param>
        /// <returns><paramref name="CategoryDto"/> remapped category.</returns>
        CategoryDto CategoryMapper(Category category);

        /// <summary>
        /// Remap a list of <paramref name="Category"/> to a list of <paramref name="CategoryDto"/>.
        /// </summary>
        /// <param name="categories">The list of categories to remap.</param>
        /// <returns>A remapped list of <paramref name="CategoryDto"/>.</returns>
        ActionResult<List<CategoryDto>> CategoryMapper(List<Category>? categories);
    }
}