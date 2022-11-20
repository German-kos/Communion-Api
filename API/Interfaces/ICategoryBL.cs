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
        /// -----
        /// </summary>
        /// <param name="creationForm">The client submitted category creation form.</param>
        /// <param name="requestor">The requestor's username for admin rights validation.</param>
        /// <returns> <paramref name="ForumCategoryDto"/> of the created category.<br/>
        /// - or - <br/>
        /// <paramref name="HTTP"/> <paramref name="Response"/> Cloudinary error.<br/>
        /// - or -<br/>
        ///  <paramref name="InternalError"/></returns>
        Task<ActionResult<ForumCategoryDto>> CreateCategory(CreateCategoryDto creationForm, string requestor);


        /// <summary>
        /// Request the category repository to delete a category from the database.
        /// </summary>
        /// <param name="deletionForm">The client submitted category deletion form.</param>
        /// <param name="requestor">The requestor's username for admin rights validation.</param>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> deletion result.<br/>
        /// - or -<br/>
        ///  <paramref name="InternalError"/></returns>
        Task<ActionResult> DeleteCategory(DeleteCategoryDto deletionForm, string requestor);
        Task<ActionResult<ForumCategoryDto>> UpdateCategory(UpdateCategoryDto categoryForm, string username);
        Task<ActionResult<ForumCategoryDto>> CreateSubCategory(CreateSubCategoryDto subCategoryForm, string username);
        Task<ActionResult<List<ForumSubCategoryDto>>> DeleteSubCategory(DeleteSubCategoryDto deleteSubCatForm, string username);
        Task<ActionResult<ForumSubCategoryDto>> UpdateSub(UpdateSubDto updateSub, string username);
        Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId); // consider moving this to a thread controller



    }
}