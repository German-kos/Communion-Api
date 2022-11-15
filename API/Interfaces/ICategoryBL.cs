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
        Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId);
        Task<ActionResult<ForumCategory>> AddCategory(CreateCategoryDto categoryForm, string username);
    }
}