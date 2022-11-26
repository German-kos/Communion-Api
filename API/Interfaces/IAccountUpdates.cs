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
        /// 
        /// </summary>
        /// <param name="editProfileForm"></param>
        /// <returns></returns>
        Task<ActionResult<AppUser>> ProcessUpdateProfile(UpdateProfileFormDto updateProfileForm);
    }
}