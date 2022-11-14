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
        Task<List<ForumCategory>> GetCategories();
        Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId);
        Task<ActionResult<ForumCategory>> AddCategory(CreateCategoryDto categoryForm, string username); // should take in a dto for creating a category
    }
}