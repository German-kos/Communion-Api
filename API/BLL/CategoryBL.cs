using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using api.Models;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;



namespace API.BLL
{
    public class CategoryBL : ICategoryBL
    {
        // Dependency Injections
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryBL(IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<ActionResult<ForumCategory>> AddCategory(CreateCategoryDto categoryForm, string username)
        {
            // Check if the request's user has rights to perform this action
            var rights = await CheckRights(username);
            if (rights != null)
                return rights.Result;

            // Check whether or not the category exists, if it does return a status code 409
            if (await _categoryRepository.GetCategoryByName(categoryForm.Name) != null)
            {
                var result = new ObjectResult("Category already exists.");
                result.StatusCode = 409;
                return result;
            }

            // If all the checks are valid, add the category to the database
            return await _categoryRepository.AddCategory(categoryForm);
        }

        public Task<List<ForumCategory>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId)
        {
            throw new NotImplementedException();
        }

        // Check if the provided username is valid, exists in the db, and is an admin
        private async Task<ActionResult<bool>> CheckRights(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (username == null || username == "" || user == null || user.IsAdmin == false)
            {
                var result = new ObjectResult("User is not authorized to perform this action.");
                result.StatusCode = 401;
                return result;
            }
            return null;
        }
    }
}