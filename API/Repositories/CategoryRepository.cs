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

        public async Task<ActionResult<ForumCategory>> AddCategory(CreateCategoryDto categoryForm)
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

            var Category = new ForumCategory
            {
                Name = categoryForm.Name,
                Info = categoryForm.Info,
                Banner = (ICollection<ForumImage>)banner
            };

            await _context.Categories.AddAsync(Category);

            await _context.SaveChangesAsync();

            return Category;
        }

        // Save changes made to the database *async
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}