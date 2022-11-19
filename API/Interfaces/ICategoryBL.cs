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
    /// The Business Logic Layer of the categories. 
    /// </summary>
    public interface ICategoryBL
    {
        /// <summary>
        /// Get an up to date lift of categories with their corresponding sub categories.
        /// </summary>
        /// <returns>A list of categories</returns>
        Task<ActionResult<List<ForumCategoryDto>>> GetAllCategories();
        Task<ActionResult<List<ForumCategoryDto>>> CreateCategory(CreateCategoryDto categoryForm, string username);
        Task<ActionResult<List<ForumCategoryDto>>> DeleteCategory(string categoryName, string username);
        Task<ActionResult<ForumCategoryDto>> UpdateCategory(UpdateCategoryDto categoryForm, string username);
        Task<ActionResult<ForumCategoryDto>> CreateSubCategory(CreateSubCategoryDto subCategoryForm, string username);
        Task<ActionResult<List<ForumSubCategoryDto>>> DeleteSubCategory(DeleteSubCategoryDto deleteSubCatForm, string username);
        Task<ActionResult<ForumSubCategoryDto>> UpdateSub(UpdateSubDto updateSub, string username);
        Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId); // consider moving this to a thread controller
    }
}