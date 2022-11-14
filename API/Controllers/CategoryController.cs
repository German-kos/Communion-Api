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

        [Authorize]
        [HttpPost("create-new-category")]
        public async Task<ActionResult<ForumCategory>> CreateCategory([FromForm] CreateCategoryDto categoryForm)
        {
            return await _categoryBL.AddCategory(categoryForm, User.GetUsername());
        }

        [HttpGet("get-category-list")]
        public async Task<ActionResult<List<ForumCategoryDto>>> GetCategories()
        {
            return await _categoryBL.GetAllCategories();
        }
    }
}