using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    /// <summary>
    /// The Data Access Layer for the categories.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retrieve a list of categories and their sub-categories from the database.<br/>-----
        /// </summary>
        /// <returns>A list of categories and their sub-categories.</returns>
        Task<List<ForumCategory>?> GetAllCategories();


        Task<ActionResult<ForumCategory>?> CreateCategory(CreateCategoryDto categoryForm);
        Task<List<ForumCategory>?> DeleteCategory(ForumCategory category);
        Task<ActionResult<ForumCategory>> UpdateCategory(ForumCategory targetCategory, UpdateCategoryDto categoryForm);
        Task<ForumCategory?> CreateSubCategory(CreateSubCategoryDto subCategoryForm, ForumCategory category);
        Task<List<ForumSubCategory>> DeleteSubCategory(DeleteSubCategoryDto deleteSubCatForm);
        Task<ForumSubCategory> UpdateSub(UpdateSubDto updateSub);
        Task<ForumCategory?> GetCategoryByName(string categoryName);


        /// <summary>
        /// Query the database if a category named <paramref name="categoryName"/> already exists.<br/>-----
        /// </summary>
        /// <param name="categoryName">The name of the category to check for it's existence.</param>
        /// <returns>
        /// True - category exists. <br/>
        /// - or - <br/>
        /// False - category does not exist.
        /// </returns>
        Task<bool> CategoryExists(string categoryName);
        Task<bool> SubCategoryExists(string categoryName, string subCategoryName);
        Task<bool> SaveAllAsync();
    }
}