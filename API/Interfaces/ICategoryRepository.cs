using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<bool> SaveAllAsync();
        Task<ActionResult<ForumCategory>> AddCategory(CreateCategoryDto categoryForm);
        Task<ForumCategory> GetCategoryByName(string categoryName);
        Task<ActionResult<List<ForumCategory>>> GetCategoryList();
        Task<ActionResult<ForumSubCategory>> AddSubCategory(CreateSubCategoryDto subCategoryForm);
    }
}