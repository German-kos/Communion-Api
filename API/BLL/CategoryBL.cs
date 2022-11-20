using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using static API.Helpers.HttpResponse;




namespace API.BLL
{
    public class CategoryBL : ICategoryBL
    {
        // Dependency Injections
        private readonly Validations _validate;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryBL(IUserRepository userRepository, ICategoryRepository categoryRepository, Validations validate)
        {
            _validate = validate;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }


        // Methods:


        public async Task<ActionResult<List<ForumCategoryDto>>> GetAllCategories()
        {
            // Pass request to data access layer, remap the get result
            var getResult = await _categoryRepository.GetAllCategories();
            return CategoryMapper(getResult);
        }


        public async Task<ActionResult<ForumCategoryDto>> CreateCategory(CreateCategoryDto creationForm, string requestor)
        {
            // Deconstruction
            string categoryName = creationForm.Name;

            // Check authorization
            if (await _validate.NotAdmin(requestor))
                return Unauthorized();

            // Check if the category exists already
            if (await _validate.CategoryExists(categoryName))
                return AlreadyExists(categoryName);

            // Pass request to data access layer, process creation result
            var creationResult = await _categoryRepository.CreateCategory(creationForm);
            return ProcessResult(creationResult);
        }


        public async Task<ActionResult> DeleteCategory(DeleteCategoryDto deletionForm, string requestor)
        {
            // Deconstruction
            var (id, name) = deletionForm;

            // Check authorization
            if (await _validate.NotAdmin(requestor))
                return Unauthorized();

            // Check if the category exists
            if (await _validate.CategoryDoesNotExist(id))
                return DoesNotExist(name);

            // Pass request to data access layer, process deletion result
            var deletionResult = await _categoryRepository.DeleteCategory(deletionForm);
            return ProcessDeletion(deletionResult, name);
        }


        public async Task<ActionResult<ForumCategoryDto>> UpdateCategory(UpdateCategoryDto updateForm, string requestor)
        {
            // Deconstruction
            var (id, name, newName, newInfo, newImageFile) = updateForm;

            // Check authorization
            if (await _validate.NotAdmin(requestor))
                return Unauthorized();

            // Check for empty form
            if (_validate.RequiredFieldsEmpty(updateForm))
                return NotModified();

            // Check if target category exists
            if (await _validate.CategoryDoesNotExist(id))
                return DoesNotExist(name);

            // Check if the new name is taken
            if (newName != null && await _validate.CategoryExists(newName))
                return AlreadyExists(newName);

            // Pass request to data access layer, process update result
            var updateResult = await _categoryRepository.UpdateCategory(updateForm);
            return ProcessResult(updateResult);
        }


        public async Task<ActionResult<ForumCategoryDto>> CreateSubCategory(CreateSubCategoryDto subCategoryForm, string username)
        {

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
            if (result == null) return NoContent(); ;

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

            return NoContent(); ;
        }
        //
        //
        //
        public async Task<ActionResult<ForumSubCategoryDto>> UpdateSub(UpdateSubDto updateSub, string username)
        {
            // Deconstruction
            var (categoryName, subName, newSubName) = updateSub;

            // Check requestor for admin rights
            var rights = await CheckRights(username);
            if (!rights.Value && rights.Result != null) return rights.Result;

            // Check if the category exists
            if (!await CategoryExists(categoryName))
                return DoesntExistResult(categoryName);

            // Check if the sub-category exists
            if (!await SubCategoryExists(categoryName, subName))
                return DoesntExistResult(subName, categoryName);

            // If all validations pass, request an update, remap the the updated sub to a suitable Dto
            // and return it
            return RemapSubCategory(await _categoryRepository.UpdateSub(updateSub));
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
        // This method recieves a sub-category from the database, remaps it to a proper Dto,
        // and returns the result
        private ForumSubCategoryDto RemapSubCategory(ForumSubCategory sub)
        {
            var (id, categoryId, name) = sub;
            return new ForumSubCategoryDto
            {
                Id = id,
                CategoryId = categoryId,
                Name = name
            };
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
                return NoContent(); ;
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
                return InternalError();

            // Check for an action result
            if (dbResponse.Result != null)
                return dbResponse.Result;

            // Check for content in the returned value
            if (dbResponse.Value != null && dbResponse.Value.Count > 0)
                return CheckReturnedList(dbResponse.Value);

            //  If both fields are empty return no content status code
            return NoContent();
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


        /// <summary>
        /// Request the category repository to query the database<br/>
        ///  for the existence of a category named <paramref name="categoryName"/>.<br/>
        /// </summary>
        /// <param name="categoryName">The name of the category to check for it's existence.</param>
        /// <returns>
        /// <paramref name="True"/> - category exists. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - category does not exist.
        /// 
        /// </returns>
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

        //---------------------------------------------------------//
        //---------------------------------------------------------//
        //----------------new and improved methods ----------------//
        //---------------------------------------------------------//
        //---------------------------------------------------------//


        /// <summary>
        /// This method takes in recieved <paramref name="ActionResult"/> recieved from the data access layer,<br/>
        /// and dermines whether to return a HTTP Response, remapped category, or an internal error.<br/>-----
        /// </summary>
        /// <param name="result">The ActionResult to process.</param>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/><br/>
        /// - or -<br/>
        /// <paramref name="ForumCategoryDto"/> Remapped category.<br/>
        /// - or -<br/>
        /// <paramref name="InternalError"/>.</returns>
        private ActionResult<ForumCategoryDto> ProcessResult(ActionResult<ForumCategory>? result)
        {
            // Check for contents
            if (result != null)
            {
                // Check for an HTTP Response
                if (result.Result != null)
                    return result.Result;

                // Check for content in the returned value
                if (result.Value != null)
                    return CategoryMapper(result.Value);
            }

            // Return an internal error if checks fail
            return InternalError();
        }


        /// <summary>
        /// This method takes in recieved <paramref name="ActionResult"/> recieved from the data access layer,<br/>
        /// and dermines whether to return a HTTP Response, or an internal error.<br/>-----
        /// </summary>
        /// <param name="result">The ActionResult to process.</param>
        /// <param name="item">The name of the deleted item.</param>
        /// <returns>
        /// <paramref name="HTTP"/> <paramref name="Response"/><br/>
        /// - or -<br/>
        /// <paramref name="InternalError"/>.
        /// </returns>
        private ActionResult ProcessDeletion(ActionResult<bool> result, string item)
        {
            // Check for contents
            if (result != null)
            {
                // Check for an HTTP Response
                if (result.Result != null)
                    return result.Result;

                // Check for deletion result
                if (result.Value)
                    return DeletionSuccessful(item);
            }

            // Return an internal error if checks fail
            return InternalError();
        }


        /// <summary>
        /// This method takes the data recieved from the data access layer and converts it to a DTO suitable for the client.<br/>-----
        /// <br/>- converts -<br/>
        /// <paramref name="ForumCategory"/><br/>
        /// - to -<br/>
        /// <paramref name="ForumCategoryDto"/> <br/>-----
        /// </summary>
        /// <param name="category">The <paramref name="ForumCategory"/> to remap.</param>
        /// <returns><paramref name="ForumCategoryDto"/> Remapped category.</returns>
        private ForumCategoryDto CategoryMapper(ForumCategory category)
        {
            // Deconstruction
            var (id, name, info, banner, subCategories) = category;

            // Return remap
            return new ForumCategoryDto
            {
                Id = id,
                Name = name,
                Info = info,
                Banner = banner,
                SubCategories = SubCategoryMapper(subCategories),
            };
        }


        /// <summary>
        /// A <paramref name="List"/> type overload for the <paramref name="CategoryMapper"/> method. Take in data recieved from the data access layer,
        /// and convert it to a Dto suitable for the client. <br/>-----<br/>
        /// - converts - <br/>
        /// <paramref name="List"/> of <paramref name="ForumCategory"/><br/>
        /// - to -<br/>
        /// <paramref name="List"/> of <paramref name="ForumCategoryDto"/> <br/>-----
        /// </summary>
        /// <param name="categories">The <paramref name="List"/> of <paramref name="ForumSubCategory"/> to remap.</param>
        /// <returns>A <paramref name="List"/> of remapped <paramref name="ForumSubCategoryDto"/><br/>
        /// - or -<br/>
        /// <paramref name="NoContent"/></returns>
        private ActionResult<List<ForumCategoryDto>> CategoryMapper(List<ForumCategory>? categories)
        {
            // Check for contents
            if (categories == null || categories.Count == 0)
                return NoContent();

            // Initializing the list
            List<ForumCategoryDto> listOfCategories = new List<ForumCategoryDto>();

            // Populating the list with remapped categories
            foreach (var category in categories)
                listOfCategories.Add(CategoryMapper(category)); // The singlular version of this mapper

            // Returning the list
            return listOfCategories;
        }


        /// <summary>
        /// This method takes the data recieved from the data access layer and converts it to a DTO suitable for the client.<br/>-----
        /// <br/>- converts -<br/>
        /// <paramref name="ForumCategory"/><br/>
        /// - to -<br/>
        /// <paramref name="ForumCategoryDto"/> <br/>-----
        /// </summary>
        /// <param name="subCategory">The <paramref name="ForumSubCategory"/> to remap.</param>
        /// <returns><paramref name="ForumSubCategoryDto"/> Remapped sub-category.</returns>
        private ForumSubCategoryDto SubCategoryMapper(ForumSubCategory subCategory)
        {
            // Deconstruction
            var (id, categoryId, name) = subCategory;

            // Return remap
            return new ForumSubCategoryDto
            {
                Id = id,
                CategoryId = categoryId,
                Name = name
            };
        }


        /// <summary>
        /// A <paramref name="List"/> type overload for the <paramref name="SubCategoryMapper"/> method. Take in data recieved from the data access layer,
        /// and convert it to a Dto suitable for the client. <br/>-----<br/>
        /// - converts - <br/>
        /// <paramref name="List"/> of <paramref name="ForumSubCategory"/><br/>
        /// - to -<br/>
        /// <paramref name="List"/> of <paramref name="ForumSubCategoryDto"/> <br/>-----
        /// </summary>
        /// <param name="subCategories">The <paramref name="List"/> of <paramref name="ForumSubCategory"/> to remap.</param>
        /// <returns>A <paramref name="List"/> of remapped <paramref name="ForumSubCategoryDto"/>.</returns>
        private List<ForumSubCategoryDto> SubCategoryMapper(List<ForumSubCategory> subCategories)
        {
            // Check for contents 
            if (subCategories == null || subCategories.Count == 0)
                return new List<ForumSubCategoryDto>(){
                new ForumSubCategoryDto{
                    Name = "No sub categories"
                }
            };

            // Initializing the list
            List<ForumSubCategoryDto> subCategoriesRemap = new List<ForumSubCategoryDto>();

            // Populating the list with remapped sub-categories
            foreach (var sub in subCategories)
                subCategoriesRemap.Add(SubCategoryMapper(sub)); // The singlular version of this mapper

            // Returning the list
            return subCategoriesRemap.OrderBy(i => i.Id).ToList();
        }

    }
}
// 466  lines of code...