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
        //
        //
        // Methods
        //
        //
        // Get a category list from the database and return it.
        public async Task<ActionResult<List<ForumCategoryDto>>> GetAllCategories()
        {
            // check if there are any categories recieved from the database
            // if none exist, return 204, no categories were found
            var categories = await _categoryRepository.GetAllCategories();
            if (categories.Value == null || categories.Value.Count == 0)
                return GenerateObjectResult(204, "No categories were found.");

            // Remap the category list to a proper Dto and return it
            return RemapCategories(categories.Value);
        }
        //
        //
        // Create a new category, add it to the database, and return an up to date category list.
        public async Task<ActionResult<List<ForumCategoryDto>>> CreateCategory(CreateCategoryDto categoryForm, string username)
        {
            // Check requestor for admin rights
            var rights = await CheckRights(username);
            if (!rights.Value && rights.Result != null) return rights.Result;

            // Check whether or not the category exists, 
            // if it already exists, return a status code 409, category already exists
            if (await _categoryRepository.GetCategoryByName(categoryForm.Name) != null)
                return GenerateObjectResult(409, "Category already exists.");

            // If all the checks are valid, add the category to the database, return the category list
            return RemapCategories(_categoryRepository.CreateCategory(categoryForm).Result.Value);
        }
        //
        //
        // Delete a category from the database by name, and return an up to date category list.
        public async Task<ActionResult<List<ForumCategoryDto>>> DeleteCategory(string categoryName, string username)
        {
            // Check requestor for admin rights
            var rights = await CheckRights(username);
            if (!rights.Value && rights.Result != null) return rights.Result;

            // Check whether or not the category exists,
            // if it doesnt, return a status code 204, category does not exist
            if (await _categoryRepository.GetCategoryByName(categoryName) == null)
                return GenerateObjectResult(204, "The category does not exist");

            // If all the checks are valid, delete the category from the database,
            // and return an up to date category list
            return RemapCategories(await _categoryRepository.DeleteCategory(categoryName));
        }
        //
        //
        // Create a new sub-category in an existing category, add it to the database,
        // and return the category with an up to date sub-category list.
        public async Task<ActionResult<ForumCategoryDto>> CreateSubCategory(CreateSubCategoryDto subCategoryForm, string username)
        {
            // Check requestor for admin rights
            var rights = await CheckRights(username);
            if (!rights.Value && rights.Result != null) return rights.Result;

            // Check whether or not the category exists,
            // if it doesnt, return status code 409, category does not exist
            var category = _categoryRepository.GetCategoryByName(subCategoryForm.CategoryName).Result.Value;
            if (category == null)
                return GenerateObjectResult(409, "Category does not exist.");

            // Check if the sub category exists in the current category,
            // if it does, return status code 409, sub category already exists
            if (CheckForSubCategory(category, subCategoryForm.Name) != null)
                return GenerateObjectResult(409, "Sub category already exists.");

            // If all the checks are valid, create a new sub category, add it to the database,
            // and return the updated category with an up to date sub-category list
            var result = _categoryRepository.CreateSubCategory(subCategoryForm, category).Result.Value;
            return RemapCategory(result);
        }
        //
        //
        // Get the threads for the requested sub-category *subject to change. *Not implemented yet.
        //
        // consider moving this to a ThreadBL instead
        public Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId)
        {
            throw new NotImplementedException();
        }
        //
        //
        // Private methods
        //
        //
        // Check if the user has admin rights.
        private async Task<ActionResult<bool>> CheckRights(string username)
        {
            // If the user doesnt exist, has an empty string for a name, or isnt an admin,
            // return 401, unauthorized
            var user = await _userRepository.GetUserByUsername(username);
            if (username == null || username == "" || user == null || user.IsAdmin == false)
                return GenerateObjectResult(401, "User is not authorized to perform this action.");


            // If the user is an admin return true
            return true;
        }
        //
        //
        // This method generates Action Results
        private ObjectResult GenerateObjectResult(int statusCode, string msg)
        {
            var result = new ObjectResult(msg);
            result.StatusCode = statusCode;
            return result;
        }
        //
        //
        // This method recieves a ForumCategory object, and maps it to a ForumCategoryDto,
        // more suitable for use in the client side
        private ForumCategoryDto RemapCategory(ForumCategory category)
        {
            // Remap the sub-categories to a suitable Dto
            var subCategoriesRemap = RemapSubCategories(category.SubCategories.ToList<ForumSubCategory>());

            // Mapping the category to a suitable Dto
            return new ForumCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Info = category.Info,
                Banner = GetBannerUrl(category),
                SubCategories = RemapSubCategories(category.SubCategories.ToList<ForumSubCategory>())
            };
        }
        //
        //
        // This method recieves a list of ForumCategory, and maps it to a list of ForumCategoryDto,
        // more suitable for use in the client side
        private List<ForumCategoryDto> RemapCategories(List<ForumCategory> categories)
        {
            // Initializing the list which will be returned
            List<ForumCategoryDto> listOfCategories = new List<ForumCategoryDto>();


            // Mapping the categories to a suitable category list Dto
            foreach (var category in categories)
            {

                listOfCategories.Add(new ForumCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Info = category.Info,
                    Banner = GetBannerUrl(category),
                    SubCategories = RemapSubCategories(category.SubCategories.ToList<ForumSubCategory>()),
                });
            }

            // return the ready category list, of ForumCategoryDto type
            return listOfCategories;
        }
        //
        //
        // This method extracts the banner url, and if it doesnt exist, it returns an empty string.
        private string GetBannerUrl(ForumCategory category)
        {
            var banner = category.Banner.LastOrDefault()?.Url;
            if (category.Banner.Count() > 0 && banner != null) return banner;
            return "";
        }
        //
        //
        // 
        private List<ForumSubCategoryDto> RemapSubCategories(List<ForumSubCategory> subCategories)
        {
            List<ForumSubCategoryDto> subCategoriesRemap = new List<ForumSubCategoryDto>();

            // If there are no sub categories, add a sub-category named "No sub-categories" 
            // to display in the client side
            if (subCategories.Count == 0)
                subCategoriesRemap.Add(new ForumSubCategoryDto { Name = "No sub-categories" });
            else
            {
                foreach (var sub in subCategories)
                    subCategoriesRemap.Add(new ForumSubCategoryDto
                    {
                        Id = sub.Id,
                        CategoryId = sub.CategoryId,
                        Name = sub.Name,
                        Threads = sub.Threads
                    });

            }
            return subCategoriesRemap.OrderBy(i => i.Id).ToList<ForumSubCategoryDto>();
        }

        // Look for a sub category in a category by name, and return it
        private ForumSubCategory CheckForSubCategory(ForumCategory category, string subCategoryName)
        {
            return category.SubCategories.FirstOrDefault(sub => sub.Name.ToLower() == subCategoryName.ToLower());
        }


    }
}