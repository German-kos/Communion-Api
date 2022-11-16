using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        // Dependency Injections
        private readonly ICategoryBL _categoryBL;
        public CategoryController(ICategoryBL categoryBL)
        {
            _categoryBL = categoryBL;
        }
        //
        [HttpGet("get-category-list")] // [GET] api/category/get-category/list
        // Get a list of the categories from the database
        public async Task<ActionResult<List<ForumCategoryDto>>> GetCategories()
        {
            return await _categoryBL.GetAllCategories();
        }
        //
        //
        [Authorize]
        [HttpPost("create-new-category")] // [POST] api/category/create-new-category
        // Create a new category
        public async Task<ActionResult<ForumCategory>> CreateCategory([FromForm] CreateCategoryDto categoryForm)
        {
            return await _categoryBL.CreateCategory(categoryForm, User.GetUsername());
        }
        //
        //
        [Authorize]
        [HttpDelete("delete-category")] // [DELETE] api/category/delete-category
        // Delete a category by name
        public async Task<ActionResult<ForumCategory>> DeleteCategory([FromForm] string categoryName)
        {
            return await _categoryBL.DeleteCategory(categoryName, User.GetUsername());
        }
        //
        [Authorize]
        [HttpPost("create-new-sub-category")] // [POST] api/category/create-new-sub-category
        // Create a new sub-category in an existing category
        public async Task<ActionResult<ForumSubCategory>> CreateSubCategory([FromForm] CreateSubCategoryDto subCategoryForm)
        {
            return await _categoryBL.AddSubCategory(subCategoryForm, User.GetUsername());
        }
    }
}