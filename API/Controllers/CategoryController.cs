using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        // Dependancy injections
        private readonly ICategoryBL _categoryBL;
        public CategoryController(ICategoryBL categoryBL)
        {
            _categoryBL = categoryBL;
        }

        [Authorize]
        [HttpPost("create-new-category")]
        public async Task<string> CreateCategory(CreateCategoryDto categoryForm)
        {
            await _categoryBL.AddCategory(categoryForm, User.GetUsername());
            return "temp return";
        }
    }
}