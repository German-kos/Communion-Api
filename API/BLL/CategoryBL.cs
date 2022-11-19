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
        //
        //
        //
        private NoContentResult _noContent = new NoContentResult();
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
            if (categories == null || categories.Count == 0)
                return GenerateObjectResult(404, "No categories were found.");

            // Remap the category list to a proper Dto and return it
            return RemapCategories(categories);
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
            // if it already exists, return 'already exists'
            if (await CategoryExists(categoryForm.Name))
                return AlreadyExistsResult(categoryForm.Name);

            // If all the checks are valid, create a new category in the database, 
            // process the response and return it
            return CheckReturnedActionResult(await _categoryRepository.CreateCategory(categoryForm));
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
            // if it doesnt, return a 'doesnt exist'
            var category = await _categoryRepository.GetCategoryByName(categoryName);
            if (category == null)
                return DoesntExistResult(categoryName);

            // If all the checks are valid, delete the category from the database,
            // process the returned value, and return it
            return CheckReturnedList(await _categoryRepository.DeleteCategory(category));
        }
        //
        //
        // Choose a category by it's name, and update it's fields. return the updated category
        public async Task<ActionResult<ForumCategoryDto>> UpdateCategory(UpdateCategoryDto categoryForm, string username)
        {
            // Check requestor for admin rights
            var rights = await CheckRights(username);
            if (!rights.Value && rights.Result != null) return rights.Result;

            // Check if the modification field are empty, if they are there's nothing to change.
            // return 304, no changes were submitted
            if (categoryForm.NewCategoryName == null && categoryForm.Info == null && categoryForm.ImageFile == null)
                return GenerateObjectResult(304, "No changes were submitted.");

            //  Check whether or not the category that's target to change exists,
            // if it doesnt, return a 'no such category'
            var category = await _categoryRepository.GetCategoryByName(categoryForm.CategoryName);
            if (category == null)
                return DoesntExistResult(categoryForm.CategoryName);

            // Check whether or not the chosen name is already in use. 
            // If it is, return 309
            if (categoryForm.NewCategoryName != null && await CategoryExists(categoryForm.NewCategoryName))
                return GenerateObjectResult(309, "The chosen category name is already in use.");

            // If all checks are valid, update the database, 
            // process the returned value, and return it

            // If all checks are valid, update the database, save the result
            var updateResult = await _categoryRepository.UpdateCategory(category, categoryForm);

            if (updateResult != null)
            {
                // If there's a action result, return it
                if (updateResult.Result != null)
                    return updateResult.Result;

                // if there's value in the result, remap and return it
                if (updateResult.Value != null)
                    return RemapCategory(updateResult.Value);
            }
            // if there was a problem with the processing of the result, return no content
            return _noContent;
        }
        //
        //
        // Create a new sub-category in an existing category, add it to the database,
        // and return the category with an up to date sub-category list.
        public async Task<ActionResult<ForumCategoryDto>> CreateSubCategory(CreateSubCategoryDto subCategoryForm, string username)
        {
            //********************************************************************************
            // rewrite the "does exist" query logic
            //********************************************************************************
            // categoryBL methods shouldnt hold data for testing until it's the final result,
            // rewrite it
            //********************************************************************************
            // also make a class / service / whatever it should be for the ObjectResults 
            // and the validations
            //********************************************************************************

            // Check requestor for admin rights
            var rights = await CheckRights(username);
            if (!rights.Value && rights.Result != null) return rights.Result;

            // Check whether or not the category exists,
            // if it doesnt, return 'does not exist'
            var category = await _categoryRepository.GetCategoryByName(subCategoryForm.CategoryName);
            if (category == null)
                return DoesntExistResult(subCategoryForm.CategoryName);

            // Check if the sub category exists in the current category,
            // if it does, return 'already exists'
            if (await SubCategoryExists(subCategoryForm.CategoryName, subCategoryForm.Name))
                return AlreadyExistsResult(subCategoryForm.Name, subCategoryForm.CategoryName);

            // Check if the response bears content
            var result = await _categoryRepository.CreateSubCategory(subCategoryForm, category);
            if (result == null) return _noContent;

            // If all the checks are valid, create a new sub category, add it to the database,
            // and return the updated category with an up to date sub-category list
            return RemapCategory(result);
        }
        //
        //
        // Delete an existing sub-category, in an existing category, update the database,
        // and return an up to date list of the remaining sub-categories (if there are any) of that category
        public async Task<ActionResult<List<ForumSubCategoryDto>>> DeleteSubCategory(DeleteSubCategoryDto deleteSubCatForm, string username)
        {
            // Check requestor for admin rights
            var rights = await CheckRights(username);
            if (!rights.Value && rights.Result != null) return rights.Result;

            // Check if the category exists
            if (!await CategoryExists(deleteSubCatForm.CategoryName))
                return DoesntExistResult(deleteSubCatForm.CategoryName);

            // Check if the sub category exists
            if (!await _categoryRepository.SubCategoryExists(deleteSubCatForm.CategoryName, deleteSubCatForm.SubCategoryName))
                return DoesntExistResult(deleteSubCatForm.SubCategoryName, deleteSubCatForm.CategoryName);

            var deletionResult = await _categoryRepository.DeleteSubCategory(deleteSubCatForm);
            if (deletionResult != null) return RemapSubCategories(deletionResult);

            return _noContent;
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

            var subCategoriesRemap = RemapSubCategories(category.SubCategories?.ToList<ForumSubCategory>());

            // Mapping the category to a suitable Dto
            return new ForumCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Info = category.Info,
                Banner = GetBannerUrl(category),
                SubCategories = RemapSubCategories(category.SubCategories?.ToList<ForumSubCategory>())
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
                var subCategoryList = category.SubCategories == null ?
                new List<ForumSubCategoryDto>() { new ForumSubCategoryDto() { Name = "No sub-categories" } }
                : RemapSubCategories(category.SubCategories.ToList<ForumSubCategory>());

                listOfCategories.Add(new ForumCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Info = category.Info,
                    Banner = GetBannerUrl(category),
                    SubCategories = subCategoryList,
                });
            }

            // return the ready category list, of ForumCategoryDto type
            return listOfCategories;
        }
        //
        //
        // This method recieves a list of ForumSubCategory, and maps it to a list of ForumSubCategoryDto,
        // more suitable for use in the client side
        private List<ForumSubCategoryDto> RemapSubCategories(List<ForumSubCategory>? subCategories)
        {
            List<ForumSubCategoryDto> subCategoriesRemap = new List<ForumSubCategoryDto>();
            // If there are no sub categories, add a sub-category named "No sub-categories" 
            // to display in the client side
            if (subCategories == null || subCategories.Count == 0)
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
        // This method recieves a category, and a sub-category name, and attempts to find the 
        // sub-category in the category
        private ForumSubCategory? CheckForSubCategory(ForumCategory category, string subCategoryName)
        {
            if (category.SubCategories == null) return null;

            return category.SubCategories.FirstOrDefault(sub => sub.Name.ToLower() == subCategoryName.ToLower());
        }
        //
        //
        // This method processes the return of an a category list from the category repository,
        // check for if it has content in it, and determine what to return
        private ActionResult<List<ForumCategoryDto>> CheckReturnedList(List<ForumCategory>? dbCategoryList)
        {
            if (dbCategoryList == null || dbCategoryList.Count == 0)
                return _noContent;
            return RemapCategories(dbCategoryList);
        }
        //
        //
        // This method processes the return of an ActionResult list of forum categories
        // and determines what to return from the content returned from the repository
        private ActionResult<List<ForumCategoryDto>> CheckReturnedActionResult(ActionResult<List<ForumCategory>?> dbResponse)
        {
            // If recieved an unexpected null action result, return status code error
            if (dbResponse == null)
                return GenerateObjectResult(500, "Something went wrong");

            // Check for an action result
            if (dbResponse.Result != null)
                return dbResponse.Result;

            // Check for content in the returned value
            if (dbResponse.Value != null && dbResponse.Value.Count > 0)
                return CheckReturnedList(dbResponse.Value);

            //  If both fields are empty return no content status code
            return _noContent;
        }
        //
        //
        // This method returns status code 409, {name} doesnt exist
        private ObjectResult DoesntExistResult(string name)
        {
            return GenerateObjectResult(409, $"\"{name}\" does not exist.");
        }
        //
        //
        // This method returns status code 409, {name} doesnt exist
        private ObjectResult DoesntExistResult(string name, string insideOf)
        {
            return GenerateObjectResult(409, $"\"{name}\" does not exist in \"{insideOf}\".");
        }
        //
        //
        // This method returns status code 409, {name} already exists
        private ObjectResult AlreadyExistsResult(string name)
        {
            return GenerateObjectResult(409, $"\"{name}\" already exists.");
        }
        //
        //
        // This method is an overload to the previous method, in case of adittional information
        private ObjectResult AlreadyExistsResult(string name, string insideOf)
        {
            return GenerateObjectResult(409, $"\"{name}\" already exists in \"{insideOf}\".");
        }
        //
        //
        // This method is just to shorten the 'CategoryExists' query from the category repository
        private async Task<bool> CategoryExists(string categoryName)
        {
            return await _categoryRepository.CategoryExists(categoryName);
        }
        //
        //
        // This method is jsut to short the 'SubCategoryExists' query from the category repository
        private async Task<bool> SubCategoryExists(string categoryName, string subCategoryName)
        {
            return await _categoryRepository.SubCategoryExists(categoryName, subCategoryName);
        }
        //
    }
}