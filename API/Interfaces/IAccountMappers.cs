using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;

namespace API.Interfaces
{
    /// <summary>
    /// This class handles account remapping for account requests.
    /// </summary>
    public interface IAccountMappers
    {
        /// <summary>
        /// Remap <paramref name="AppUser"/> to <paramref name="SignedInUserDto"/>.
        /// </summary>
        /// <param name="user">The <paramref name="AppUser"/> to remap.</param>
        /// <returns><paramref name="SignedInUserDto"/> remapped user. </returns>
        SignedInUserDto AccountMapper(AppUser user);

        /// <summary>
        /// Remap <paramref name="AppUser"/> to <paramref name="SignedInUserDto"/>.
        /// </summary>
        /// <param name="user">The <paramref name="AppUser"/> to remap.</param>
        /// <param name="remember">Whether to remember the signed in user or not</param>
        /// <returns><paramref name="SignedInUserDto"/> remapped user. </returns>
        SignedInUserDto AccountMapper(AppUser user, bool remember);

        /// <summary>
        /// Remap <paramref name="ProfilePicture"/> to <paramref name="ProfilePictureDto"/>.
        /// </summary>
        /// <param name="pfp">The <paramref name="ProfilePicture"/> to remap.</param>
        /// <returns><paramref name="ProfilePictureDto"/> remapped profile picture.</returns>
        ProfilePictureDto ProfilePictureMapper(ProfilePicture pfp);
    }
}