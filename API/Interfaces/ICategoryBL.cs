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
    /// The Business Logic Layer for the categories. 
    /// </summary>
    public interface ICategoryBL
    {
        /// <summary>
        /// Get an up to date list of categories with their corresponding sub-categories.<br/>
        /// ------
        /// </summary>
        /// <returns>A list of categories (ForumCategoryDto).</returns>
        Task<ActionResult<List<ForumCategoryDto>>> GetAllCategories();
        /// <summary>
        /// Request the category repository to create and add a new category to the database.<br/>
        /// -----<br/>
        /// The method recieves the client submitted category creation form,<br/>
        /// and the requestor's username for admin rights validation.<br/>
        /// -----
        /// </summary>
        /// <param name="categoryForm">The client submitted category creation form.</param>
        /// <param name="username">The requestor's username for admin rights validation</param>
        /// <returns>The category that has been created and added.
        /// <para> - or - </para>
        /// The HTTP Response of an error that occurred.</returns>
        Task<ActionResult<List<ForumCategoryDto>>> CreateCategory(CreateCategoryDto categoryForm, string username);
        Task<ActionResult<List<ForumCategoryDto>>> DeleteCategory(string categoryName, string username);
        Task<ActionResult<ForumCategoryDto>> UpdateCategory(UpdateCategoryDto categoryForm, string username);
        Task<ActionResult<ForumCategoryDto>> CreateSubCategory(CreateSubCategoryDto subCategoryForm, string username);
        Task<ActionResult<List<ForumSubCategoryDto>>> DeleteSubCategory(DeleteSubCategoryDto deleteSubCatForm, string username);
        Task<ActionResult<ForumSubCategoryDto>> UpdateSub(UpdateSubDto updateSub, string username);
        Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId); // consider moving this to a thread controller
    }
}