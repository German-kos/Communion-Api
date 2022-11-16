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
        //
        // Controllers
        //
        //
        [HttpGet("get-category-list")] // [GET] api/category/get-category/list
        public async Task<ActionResult<List<ForumCategoryDto>>> GetCategories()
        {
            // Get a list of the categories from the database.
            return await _categoryBL.GetAllCategories();
        }
        //
        //
        //
        [Authorize] // Role of an admin is required
        [HttpPost("create-new-category")] // [POST] api/category/create-new-category
        public async Task<ActionResult<ForumCategory>> CreateCategory([FromForm] CreateCategoryDto categoryForm)
        {
            // Create a new category, return an updated category list.
            return await _categoryBL.CreateCategory(categoryForm, User.GetUsername());
        }
        //
        //
        //
        [Authorize] // Role of an admin is required
        [HttpDelete("delete-category")] // [DELETE] api/category/delete-category
        public async Task<ActionResult<List<ForumCategoryDto>>> DeleteCategory([FromForm] string categoryName)
        {
            // Delete a category by name, and return an updated category list.
            return await _categoryBL.DeleteCategory(categoryName, User.GetUsername());
        }
        //
        //
        //
        [Authorize] // Role of an admin is required
        [HttpPost("create-new-sub-category")] // [POST] api/category/create-new-sub-category
        public async Task<ActionResult<ForumCategoryDto>> CreateSubCategory([FromForm] CreateSubCategoryDto subCategoryForm)
        {
            // Create a sub category in the requested category, return the category with updated sub-category list
            return await _categoryBL.CreateSubCategory(subCategoryForm, User.GetUsername());
        }
    }
}