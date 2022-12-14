using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Data;
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


        // Controllers


        [HttpGet("get-category-list")] // [GET] api/category/get-category/list
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            // Get a list of the categories from the database.
            return await _categoryBL.GetAllCategories();
        }


        [Authorize] // Role of an admin is required
        [HttpPost("create-new-category")] // [POST] api/category/create-new-category
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromForm] CreateCategoryDto categoryForm)
        {
            // Create a new category, return an updated category list.
            return await _categoryBL.CreateCategory(categoryForm, User.GetUsername());
        }


        [Authorize] // Role of an admin is required
        [HttpDelete("delete-category")] // [DELETE] api/category/delete-category
        public async Task<ActionResult> DeleteCategory([FromForm] DeleteCategoryDto deletionForm)
        {
            // Delete a category by name, and return an updated category list.
            return await _categoryBL.DeleteCategory(deletionForm, User.GetUsername());
        }


        [Authorize] // Role of an admin is required
        [HttpPatch("edit-category")] // [PATCH] api/catagory/edit-category
        public async Task<ActionResult<CategoryDto>> UpdateCategory([FromForm] UpdateCategoryDto updateForm)
        {
            // Update a category by name, return the updated category.
            return await _categoryBL.UpdateCategory(updateForm, User.GetUsername());
        }


        [Authorize] // Role of an admin is required
        [HttpPost("create-new-sub-category")] // [POST] api/category/create-new-sub-category
        public async Task<ActionResult<CategoryDto>> CreateSubCategory([FromForm] CreateSubCategoryDto subCategoryForm)
        {
            // Create a sub category in the requested category, return the category with updated sub-category list
            return await _categoryBL.CreateSubCategory(subCategoryForm, User.GetUsername());
        }


        [Authorize] // Role of an admin is required
        [HttpDelete("delete-sub-category")] // [DELETE] api/category/delete-sub-category
        public async Task<ActionResult<List<ForumSubCategoryDto>>> DeleteSubCategory([FromForm] DeleteSubCategoryDto deleteSubCatForm)
        {
            // Delete a category by name, and return an updated category list.
            return await _categoryBL.DeleteSubCategory(deleteSubCatForm, User.GetUsername());
        }


        [Authorize] // Role of an admin is required
        [HttpPatch("edit-sub-category")] // [PATCH] api/catagory/edit-sub-category
        public async Task<ActionResult<ForumSubCategoryDto>> UpdateSub([FromForm] UpdateSubDto updateSub)
        {
            // Update a category by name, return the updated category.
            return await _categoryBL.UpdateSub(updateSub, User.GetUsername());
        }
    }
}