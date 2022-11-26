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
    /// This class handles updates for account requests.
    /// </summary>
    public interface IAccountUpdates
    {
        /// <summary>
        /// Process the client submitted profile update form.
        /// </summary>
        /// <param name="editProfileForm">The client submitted updated profile form.</param>
        /// <returns>A remapped user with the new information.</returns>
        Task<ActionResult<ProfileInformationDto>> ProcessUpdateProfile(UpdateProfileFormDto updateProfileForm);
    }
}