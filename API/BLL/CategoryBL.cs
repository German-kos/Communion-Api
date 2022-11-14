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

        // Get the categories with their sub-categories
        public async Task<ActionResult<List<ForumCategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetCategoryList();
            if (categories == null)
                return GenerateObjectResult(204, "No categories were found.");
            List<ForumCategoryDto> categoryList = RemapCategories(categories.Value);
            return categoryList;
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
                return GenerateObjectResult(401, "User is not authorized to perform this action.");
            }
            return null;
        }

        private ObjectResult GenerateObjectResult(int statusCode, string message)
        {
            var result = new ObjectResult(message);
            result.StatusCode = statusCode;
            return result;
        }

        private List<ForumCategoryDto> RemapCategories(List<ForumCategory> categories)
        {
            List<ForumCategoryDto> listOfCategories = new List<ForumCategoryDto>();
            //
            foreach (var category in categories)
            {

                // This is a test to check if the current category has a banner picture.
                string banner = "";
                if (category.Banner.Count() > 0)
                    banner = category.Banner.LastOrDefault().Url;

                // If there are no sub categories, add a sub-category named "No sub-categories" 
                // to display in the client side
                List<ForumSubCategory> subCategories = category.SubCategories.ToList();
                if (category.SubCategories.Count == 0)
                    category.SubCategories.Add(new ForumSubCategory
                    {
                        Name = "No sub-categories",

                    });

                // Mapping the category to a dto for the client side
                listOfCategories.Add(new ForumCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Info = category.Info,
                    Banner = banner,
                    SubCategories = category.SubCategories.ToList<ForumSubCategory>()
                });
            }

            return listOfCategories;
        }
    }
}