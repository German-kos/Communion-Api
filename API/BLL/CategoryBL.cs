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
            if (await CheckRights(username))
            {
                return await _categoryRepository.AddCategory(categoryForm);
            }
            var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Something went wrong." };
            throw new HttpResponseException(msg);
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
        private async Task<bool> CheckRights(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (username == null || username == "" || user == null || !user.IsAdmin)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            return true;
        }
    }
}