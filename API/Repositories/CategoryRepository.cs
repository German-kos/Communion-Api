using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using api.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        //
        private NoContentResult _noContent = new NoContentResult();

        // Dependency Injections
        private readonly DataContext _context;
        private readonly IImageService _imageService;
        public CategoryRepository(DataContext context, IImageService imageService)
        {
            _imageService = imageService;
            _context = context;
        }
        //
        //
        //
        //
        //
        // Methods
        //
        //
        // Get the categories from the database
        public async Task<List<ForumCategory>?> GetAllCategories()
        {
            // Return a list of categories, each category includes it's collection of banners and sub-categories

            return await _context.Categories
            .Include(c => c.Banner)
            .Include(c => c.SubCategories)
            .ToListAsync<ForumCategory>();
        }
        //
        //
        // Create a category and add it to the database
        public async Task<ActionResult<List<ForumCategory>?>> CreateCategory(CreateCategoryDto categoryForm)
        {
            // Uploading the image to the cloudinary api
            var bannerUploadResult = await _imageService.UploadImageAsync(categoryForm.ImageFile);

            // Check if the upload fails
            // If it does, return the error
            if (bannerUploadResult.Error != null)
            {
                var result = new ObjectResult(bannerUploadResult.Error.Message);
                result.StatusCode = 400;
                return result;
            }

            // Initializing a ForumImage with the upload result
            var banner = new ForumImage
            {
                Url = bannerUploadResult.SecureUrl.AbsoluteUri,
                PublicId = bannerUploadResult.PublicId
            };

            // Initializing a collection for the category's banners
            // Then add the banner to the collection
            List<ForumImage> bannerCol = new List<ForumImage>();
            bannerCol.Add(banner);

            // Creating a new category, and adding it to the database
            await _context.Categories.AddAsync(new ForumCategory
            {
                Name = categoryForm.Name,
                Info = categoryForm.Info,
                Banner = bannerCol
            });
            await SaveAllAsync();

            // Return an up to date category list
            var categoryList = await GetAllCategories();
            return categoryList;
        }
        //
        //
        // Delete the requested category from the database
        public async Task<List<ForumCategory>?> DeleteCategory(ForumCategory targetCategory)
        {
            // Remove the targeted row from the database
            _context.Categories.Remove(targetCategory);
            await SaveAllAsync();

            // Return an up to date category list
            return await GetAllCategories();
        }
        //
        //
        // Create a sub-category in an existing category, and add it to the database
        public async Task<ForumCategory?> CreateSubCategory(CreateSubCategoryDto subCategoryForm, ForumCategory category)
        {
            // Initializing a new sub-category, add it to the sub-category collection in the requested category,
            // and add it to the database
            category.SubCategories.Add(new ForumSubCategory
            {
                Name = subCategoryForm.Name
            });
            await SaveAllAsync();

            // Return the category with an up to date sub-category list

            return await GetCategoryByName(category.Name);
        }
        //
        //
        // Find a category by name in the database
        public async Task<ForumCategory?> GetCategoryByName(string categoryName)
        {
            var category = await _context.Categories
            .Include(c => c.SubCategories)
            .Include(c => c.Banner)
            .FirstOrDefaultAsync(category => category.Name.ToLower() == categoryName.ToLower());

            return category;
        }
        //
        //
        //
        // Save changes made to the database
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        //
    }
}