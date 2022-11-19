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
        Task<ActionResult<ForumCategory>> UpdateCategory(ForumCategory targetCategory, UpdateCategoryDto categoryForm);
        Task<ForumCategory?> CreateSubCategory(CreateSubCategoryDto subCategoryForm, ForumCategory category);
        Task<List<ForumSubCategory>> DeleteSubCategory(DeleteSubCategoryDto deleteSubCatForm);
        Task<ForumSubCategory> UpdateSub(UpdateSubDto updateSub);
        Task<ForumCategory?> GetCategoryByName(string categoryName);
        Task<bool> CategoryExists(string categoryName);
        Task<bool> SubCategoryExists(string categoryName, string subCategoryName);
        Task<bool> SaveAllAsync();
    }
}