using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
        public CategoryBL(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ActionResult<ForumCategory>> AddCategory(CreateCategoryDto categoryForm, string username)
        {
            // var user = await _userRepository.GetUserByUsername(username);
            // if (username == null || username == "" || user == null || !user.IsAdmin)
            //     throw new HttpResponseException(HttpStatusCode.Unauthorized);
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

        Task<ForumCategory> ICategoryBL.AddCategory(CreateCategoryDto categoryForm, string username)
        {
            throw new NotImplementedException();
        }
    }
}