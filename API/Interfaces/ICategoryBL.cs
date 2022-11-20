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
        /// Retrieve an up to date list of categories with their corresponding sub-categories.<br/>
        /// ------
        /// </summary>
        /// <returns>A <paramref name="List"/> of <paramref name="ForumCategoryDto"/>.<br/>
        /// - or - <br/>
        /// <paramref name="NoContent"/></returns>
        Task<ActionResult<List<ForumCategoryDto>>> GetAllCategories();


        /// <summary>
        /// Request the category repository to create and add a new category to the database.<br/>
        /// -----<br/>
        /// The method recieves the client submitted category creation form,<br/>
        /// and the requestor's username for admin rights validation.<br/>
        /// -----
        /// </summary>
        /// <param name="createCategory">The client submitted category creation form.</param>
        /// <param name="username">The requestor's username for admin rights validation</param>
        /// <returns>The category that has been created and added.
        /// <para> - or - </para>
        /// The HTTP Response of an error that occurred to the database.</returns>
        Task<ActionResult<ForumCategoryDto>> CreateCategory(CreateCategoryDto createCategory, string username);
        Task<ActionResult<List<ForumCategoryDto>>> DeleteCategory(string categoryName, string username);
        Task<ActionResult<ForumCategoryDto>> UpdateCategory(UpdateCategoryDto categoryForm, string username);
        Task<ActionResult<ForumCategoryDto>> CreateSubCategory(CreateSubCategoryDto subCategoryForm, string username);
        Task<ActionResult<List<ForumSubCategoryDto>>> DeleteSubCategory(DeleteSubCategoryDto deleteSubCatForm, string username);
        Task<ActionResult<ForumSubCategoryDto>> UpdateSub(UpdateSubDto updateSub, string username);
        Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId); // consider moving this to a thread controller



    }
}