using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static API.Helpers.HttpResponse;

namespace API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        //
        //
        // Dependency Injections
        private readonly DataContext _context;
        private readonly IImageService _imageService;
        public CategoryRepository(DataContext context, IImageService imageService)
        {
            _imageService = imageService;
            _context = context;
        }


        // Methods


        public async Task<List<Category>?> GetAllCategories()
        {
            // Return a list of categories, each category includes it's collection of banners and sub-categories
            return await _context.Categories
            .Include(c => c.Banner)
            .Include(c => c.SubCategories)
            .ToListAsync<Category>();
        }


        public async Task<ActionResult<Category>?> CreateCategory(CreateCategoryDto creationForm)
        {
            // Deconstruction
            var (name, info, imageFile) = creationForm;

            // Upload the banner image
            var bannerCol = await UploadBanner(imageFile);

            // Null checks
            if (bannerCol == null || bannerCol.Value == null)
                return InternalError();
            if (bannerCol.Result != null)
                return bannerCol.Result;

            // Creating a new category, and adding it to the database
            var creationResult = await _context.Categories.AddAsync(new Category
            {
                Name = name,
                Info = info,
                Banner = bannerCol.Value
            });

            //return created category
            if (await SaveAllAsync())
                return creationResult.Entity;

            // if failed to save
            return InternalError();
        }


        public async Task<ActionResult<bool>> DeleteCategory(DeleteCategoryDto deletionForm)
        {
            // Deconstruction
            var (id, name) = deletionForm;

            // Find the target
            var targetCategory = await GetCategoryById(id);

            // Check if exists
            if (targetCategory == null)
                return DoesNotExist(name);

            // Banner publicID for deletion
            var bannerPublicId = targetCategory.Banner.PublicId;

            // Delete the category from the database
            _context.Categories.Remove(targetCategory);

            bool deletionResult = await SaveAllAsync();

            // If deletion fails
            if (!deletionResult)
                return InternalError();

            // Delete banner from Cloudinary
            await _imageService.DeleteImageAsync(bannerPublicId);

            // Return result
            return deletionResult;
        }


        public async Task<ActionResult<Category>> UpdateCategory(UpdateCategoryDto updateForm)
        {
            // Deconstruction
            var (id, name, newName, newInfo, newImageFile) = updateForm;

            // Find the target
            var targetCategory = await GetCategoryById(id);

            // If category does not exist
            if (targetCategory == null)
                return DoesNotExist(name);

            // Track the entity
            _context.Categories.Attach(targetCategory);


            // If image file exists, update it
            if (newImageFile != null)
            {
                // Save previous banner publicId to later remove from Cloudinary
                var previousBanner = targetCategory.Banner.PublicId;

                // Upload the banner image
                var banner = await UploadBanner(newImageFile);

                // Null checks
                if (banner == null || banner.Value == null)
                    return InternalError();
                if (banner.Result != null)
                    return banner.Result;

                // Apply banner change to the target category
                targetCategory.Banner = banner.Value;

                // Note modification
                _context.Entry(targetCategory).Property(c => c.Banner).IsModified = true;

                // Delete old banner from Cloudinary
                await _imageService.DeleteImageAsync(previousBanner);
            }

            // If new name exists, update it
            if (newName != null && newName != targetCategory.Name)
            {
                // Changing the name
                targetCategory.Name = newName;

                // Note modification
                _context.Entry(targetCategory).Property(c => c.Name).IsModified = true;
            }

            // If new info exists, update it
            if (newInfo != null && newInfo != targetCategory.Info)
            {
                // Changing the info
                targetCategory.Info = newInfo;

                // Note modification
                _context.Entry(targetCategory).Property(c => c.Info).IsModified = true;
            }


            // If nothing was modified
            if (!await SaveAllAsync())
                return NotModified();


            // Return the updated category
            return targetCategory;
        }
        //
        //
        // Create a sub-category in an existing category, and add it to the database
        public async Task<Category?> CreateSubCategory(CreateSubCategoryDto subCategoryForm, Category category)
        {
            // Initializing a new sub-category, add it to the sub-category collection in the requested category,
            // and add it to the database
            category.SubCategories.Add(new SubCategory
            {
                Name = subCategoryForm.Name
            });
            await SaveAllAsync();

            // Return the category with an up to date sub-category list
            return await GetCategoryByName(category.Name);
        }
        //
        //
        // Delete a sub category from an existing category
        public async Task<List<SubCategory>> DeleteSubCategory(DeleteSubCategoryDto deleteSubCatForm)
        {
            // Find the category in the database
            var category = await _context.Categories
            .Include(c => c.SubCategories)
            .FirstAsync(c => c.Name.ToLower() == deleteSubCatForm.CategoryName.ToLower());

            // Remove the requested sub-category from the category
            category.SubCategories
            .Remove(category.SubCategories.First(sub => sub.Name.ToLower() == deleteSubCatForm.SubCategoryName.ToLower()));

            await SaveAllAsync();

            // Return the remaining sub-categories
            return category.SubCategories.ToList<SubCategory>();
        }
        //
        //
        // Update the requested sub-category in the database
        public async Task<SubCategory> UpdateSub(UpdateSubDto updateSub)
        {
            // Deconstructing the recieved form
            var (categoryName, subName, newSubName) = updateSub;

            // Find the sub category in the database
            var category = await _context.Categories
            .Include(c => c.SubCategories)
            .FirstAsync(c => c.Name.ToLower() == categoryName.ToLower());

            var sub = category.SubCategories.First(s => s.Name.ToLower() == subName);

            // Track the entity
            _context.Attach(sub);

            // Apply changes
            sub.Name = newSubName;

            // Note modification
            _context.Entry(sub).Property(s => s.Name).IsModified = true;

            await SaveAllAsync();

            // return the modified sub-category
            return sub;
        }
        //
        //
        // Find a category by name in the database
        public async Task<Category?> GetCategoryByName(string categoryName)
        {
            var category = await _context.Categories
            .Include(c => c.SubCategories)
            .Include(c => c.Banner)
            .FirstOrDefaultAsync(category => category.Name.ToLower() == categoryName.ToLower());

            return category;
        }


        public Task<bool> CategoryExists(string categoryName)
        {
            return _context.Categories
            .AnyAsync(c => c.Name.ToLower() == categoryName.ToLower());
        }


        public async Task<bool> CategoryExists(int categoryId)
        {
            return await _context.Categories
            .AnyAsync(c => c.Id == categoryId);
        }
        //
        //
        // Check if a sub-category exists
        public async Task<bool> SubCategoryExists(string categoryName, string subCategoryName)
        {
            return await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == categoryName.ToLower()
            && c.SubCategories
            .Any(sub => sub.Name.ToLower() == subCategoryName.ToLower()));
        }

        //
        //
        //
        /// <summary>
        /// Commit changes to database.<br/>-----
        /// </summary>
        /// <returns>
        /// <paramref name="True"/> - on success <br/>
        /// - or -<br/>
        /// <paramref name="false"/> - on failure
        /// </returns>
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


        //---------------------------------------------------------//
        //---------------------------------------------------------//
        //----------------new and improved methods ----------------//
        //-------------------------helpers-------------------------//
        //---------------------------------------------------------//
        //---------------------------------------------------------//


        /// <summary>
        /// Retrieve a category from the database by ID.<br/>
        /// Includes the category's banners and sub categories.<br/>
        /// -----
        /// </summary>
        /// <param name="id">The id of the category.</param>
        /// <returns><paramref name="ForumCategory"/></returns>
        private async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories
            .Include(c => c.Banner)
            .FirstOrDefaultAsync(c => c.Id == id);
        }


        /// <summary>
        /// Uploading a banner image to the Cloudinary api.<br/>
        /// -----
        /// </summary>
        /// <param name="image">The image file to upload</param>
        /// <returns><paramref name="List"/> of <paramref name="ForumImage"/></returns>
        private async Task<ActionResult<Banner>> UploadBanner(IFormFile image)
        {
            // Uploading the banner image to the cloudinary api
            var uploadResult = await _imageService.UploadBannerAsync(image);

            // Check if the upload fails
            if (uploadResult.Error != null)
                return GenerateResponse(400, uploadResult.Error.Message);

            // Return the collection with the with the upload result
            return new Banner
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };
        }
    }
}