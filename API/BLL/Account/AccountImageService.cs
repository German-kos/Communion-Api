using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.BLL.Account
{
    public class AccountImageService : IAccountImageService
    {
        // Dependency Injections
        private readonly IAccountRepository _repo;
        private readonly IImageService _img;
        private readonly IAccountMappers _map;
        public AccountImageService(IAccountRepository repo, IImageService img, IAccountMappers map)
        {
            _map = map;
            _img = img;
            _repo = repo;
        }


        // Methods:


        public async Task<ActionResult<ProfilePictureDto>> UploadPfp(UploadPfpFormDto uploadPfpForm)
        {
            // Deconstruction
            var (username, profilePicture) = uploadPfpForm;

            var user = await _repo.GetUserIncludePfp(username);

            if (user == null)
                return new NotFoundObjectResult("User does not exist");

            var uploadResult = await _img.UploadImageAsync(profilePicture);

            if (uploadResult.Error != null)
                return new BadRequestObjectResult(uploadResult.Error.Message);

            var newPfp = new ProfilePicture
            {
                User = user,
                UserId = user.Id,
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };

            string? previousPfpId = "";

            if (user.ProfilePicture != null)
                previousPfpId = user.ProfilePicture.PublicId;

            user.ProfilePicture = newPfp;

            if (await _repo.SaveAllAsync() && previousPfpId != "")
                await _img.DeleteImageAsync(previousPfpId);

            return _map.ProfilePictureMapper(user.ProfilePicture);
        }
    }
}