using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<ForumCategory>?> GetAllCategories();
        Task<ActionResult<List<ForumCategory>?>> CreateCategory(CreateCategoryDto categoryForm);
        Task<List<ForumCategory>?> DeleteCategory(ForumCategory category);
        Task<ForumCategory?> GetCategoryByName(string categoryName);
        Task<ForumCategory?> CreateSubCategory(CreateSubCategoryDto subCategoryForm, ForumCategory category);
        Task<bool> SaveAllAsync();
    }
}