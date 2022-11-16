using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ICategoryBL
    {
        Task<ActionResult<List<ForumCategoryDto>>> GetAllCategories();
        Task<ActionResult<ForumCategory>> CreateCategory(CreateCategoryDto categoryForm, string username);
        Task<ActionResult<List<ForumCategoryDto>>> DeleteCategory(string categoryName, string username);
        Task<ActionResult<ForumSubCategory>> AddSubCategory(CreateSubCategoryDto subCategoryForm, string username);
        Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId);
    }
}