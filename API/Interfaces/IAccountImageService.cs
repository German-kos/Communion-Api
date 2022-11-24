using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    /// <summary>
    /// This class handles image services for account requests.
    /// </summary>
    public interface IAccountImageService
    {
        /// <summary>
        /// rocess the upload pfp request, if the request is valid upload the picture to the database, add it to the corresponding requestor.
        /// </summary>
        /// <param name="uploadPfpForm">The client submitted upload pfp form.</param>
        /// <returns><paramref name="ProfilePicture"/> of the added profile picture.</returns>
        Task<ActionResult<ProfilePictureDto>> UploadPfp(UploadPfpFormDto uploadPfpForm);
    }
}