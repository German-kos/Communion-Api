using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    /// <summary>
    /// Account Data Access Layer.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Add a new user to the database.
        /// </summary>
        /// <param name="newUser">The user to add to the database.</param>
        /// <returns>
        ///  <paramref name="SignedInUserDto"/> of the newly added user.
        /// </returns>
        Task<ActionResult<AppUser>> SignUp(AppUser newUser);

        /// <summary>
        /// Query the database for if a user exists.
        /// </summary>
        /// <param name="username">The username to look for.</param>
        /// <returns>
        /// <paramref name="True"/> - if the user exists. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - if the user doesn't exist.
        /// </returns>
        Task<bool> DoesUserExist(string username);

        /// <summary>
        /// Query the database for if an email exists.
        /// </summary>
        /// <param name="email">The email to look for.</param>
        /// <returns>
        /// <paramref name="True"/> - if the email exists. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - if the email doesn't exist.
        /// </returns>
        Task<bool> DoesEmailExist(string email);

        /// <summary>
        /// Query the database for a user by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user corresponding to the username.</returns>
        Task<AppUser?> GetUserByUsername(string username);

        /// <summary>
        /// Query the database for a user by username, include the profile picture.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user corresponding to the username, including the profile picture.</returns>
        Task<AppUser?> GetUserIncludePfp(string username);

        /// <summary>
        /// Query the database for a user by id.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user corresponding to the id.</returns>
        Task<AppUser?> GetUserById(int id);

        // need to document
        /// <summary>
        /// Update a user in the database with new information submitted by the user.
        /// </summary>
        /// <param name="updateProfileForm">The client submitted update profile form.</param>
        /// <param name="fieldsToUpdate">The properties in the user information that need to be updated.</param>
        /// <returns>The updated user with the new information.</returns>
        Task<ActionResult<AppUser>> UpdateProfile(UpdateProfileFormDto updateProfileForm, List<PropertyInfo> fieldsToUpdate);

        /// <summary>
        /// Save changes made to the database.
        /// </summary>
        /// <returns>
        /// <paramref name="True"/> - if saved successfully. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - if there was a problem saving.
        /// </returns>
        Task<bool> SaveAllAsync();
    }
}