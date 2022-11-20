using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IImageService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<ImageUploadResult> UploadBannerAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}