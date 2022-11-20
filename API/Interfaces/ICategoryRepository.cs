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
        /// <returns>A <paramref name="List"/> of <paramref name="ForumCategory"/>.</returns>
        Task<List<ForumCategory>?> GetAllCategories();


        /// <summary>
        /// Create a new category and add it to the database.<br/>-----
        /// 
        /// 
        /// </summary>
        /// <param name="creationForm"></param>
        /// <returns></returns>
        Task<ActionResult<ForumCategory>?> CreateCategory(CreateCategoryDto creationForm);


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