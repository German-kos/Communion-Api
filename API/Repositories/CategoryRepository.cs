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
        // Methods
        //
        //
        // Get the categories from the database
        public async Task<ActionResult<List<ForumCategory>>> GetAllCategories()
        {
            return await _context.Categories
            .Include(c => c.Banner)
            .Include(c => c.SubCategories)
            .ToListAsync<ForumCategory>();
        }
        //
        //
        //
        public async Task<ActionResult<ForumCategory>> CreateCategory(CreateCategoryDto categoryForm)
        {
            var bannerUploadResult = await _imageService.UploadImageAsync(categoryForm.ImageFile);

            if (bannerUploadResult.Error != null)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = bannerUploadResult.Error.Message };
                throw new HttpResponseException(msg);
            }

            var banner = new ForumImage
            {
                Url = bannerUploadResult.SecureUrl.AbsoluteUri,
                PublicId = bannerUploadResult.PublicId
            };

            List<ForumImage> bannerCol = new List<ForumImage>();
            bannerCol.Add(banner);

            var Category = new ForumCategory
            {
                Name = categoryForm.Name,
                Info = categoryForm.Info,
                Banner = bannerCol
            };

            await _context.Categories.AddAsync(Category);

            await _context.SaveChangesAsync();

            return Category;
        }
        //
        //
        //
        public async Task<bool> DeleteCategory(string categoryName)
        {
            // Find the targeted row in the database
            var removeTarget = _context.Categories
            .FirstOrDefault(category => category.Name.ToLower() == categoryName.ToLower());

            // Remove the targeted row from the database
            _context.Categories.Remove(removeTarget);

            // return what was deleted
            return await SaveAllAsync();
        }
        //
        //
        //
        public async Task<ActionResult<ForumSubCategory>> AddSubCategory(CreateSubCategoryDto subCategoryForm, ForumCategory category)
        {
            category.SubCategories.Add(new ForumSubCategory
            {
                Name = subCategoryForm.Name
            });
            await SaveAllAsync();
            return category.SubCategories
            .FirstOrDefault(sub => sub.Name.ToLower() == subCategoryForm.Name.ToLower());
        }

        public async Task<ForumCategory> GetCategoryByName(string categoryName)
        {
            return await _context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(category => category.Name.ToLower() == categoryName.ToLower());
        }




        // Save changes made to the database *async
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


    }
}