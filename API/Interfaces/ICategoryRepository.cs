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
        /// </summary>
        /// <param name="creationForm">The client submitted category creation form.</param>
        /// <returns><paramref name="ForumCategory"/> of the created category.</returns>
        Task<ActionResult<ForumCategory>?> CreateCategory(CreateCategoryDto creationForm);


        /// <summary>
        /// Remove a category from the database.<br/>-----
        /// </summary>
        /// <param name="deletionForm">The client submitted category deletion form.</param>
        /// <returns>
        /// <paramref name="True"/> - deletion successful.<br/>
        /// - or - <br/>
        ///  <paramref name="False"/> - deletion failed.
        /// </returns>
        Task<ActionResult<bool>> DeleteCategory(DeleteCategoryDto deletionForm);


        /// <summary>
        /// Update an existing category in the database.<br/>-----
        /// </summary>
        /// <param name="updateForm">The client submitted category update form.</param>
        /// <returns><paramref name="ForumCategory"/> of the updated category.</returns>
        Task<ActionResult<ForumCategory>> UpdateCategory(UpdateCategoryDto updateForm);
        Task<ForumCategory?> CreateSubCategory(CreateSubCategoryDto subCategoryForm, ForumCategory category);
        Task<List<ForumSubCategory>> DeleteSubCategory(DeleteSubCategoryDto deleteSubCatForm);
        Task<ForumSubCategory> UpdateSub(UpdateSubDto updateSub);
        Task<ForumCategory?> GetCategoryByName(string categoryName);


        /// <summary>
        /// Query the database if a category named <paramref name="categoryName"/> already exists.<br/>-----
        /// </summary>
        /// <param name="categoryName">The name of the category to check the existence of.</param>
        /// <returns>
        /// <paramref name="True"/> - category exists. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - category does not exist.
        /// </returns>
        Task<bool> CategoryExists(string categoryName);

        /// <summary>
        /// Query the database if a category with the provided <paramref name="categoryId"/> already exists.<br/>-----
        /// </summary>
        /// <param name="categoryId">The id of the category to check the existence of.</param>
        /// <returns>
        /// <paramref name="True"/> - category exists. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - category does not exist.
        /// </returns>
        Task<bool> CategoryExists(int categoryId);
        Task<bool> SubCategoryExists(string categoryName, string subCategoryName);
        Task<bool> SaveAllAsync();
    }
}