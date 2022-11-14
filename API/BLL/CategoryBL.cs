using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;

namespace API.BLL
{
    public class CategoryBL : ICategoryBL
    {
        public Task<ForumCategory> AddCategory()
        {
            throw new NotImplementedException();
        }

        public Task<ForumCategory> AddCategory(CreateCategoryDto categoryForm, string username)
        {
            throw new NotImplementedException();
        }

        public Task<List<ForumCategory>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId)
        {
            throw new NotImplementedException();
        }
    }
}