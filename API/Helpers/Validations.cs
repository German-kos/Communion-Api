using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;

namespace API.Helpers
{
    /// <summary>
    /// A helper singleton for various validations
    /// </summary>
    public class Validations
    {
        /// <summary>
        /// Dependency Injections
        /// </summary>
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        public Validations(IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }


        // Methods:


        /// <summary>
        /// Request the user repository to query whether or not the given user
        /// has admin rights.<br/>-----
        /// </summary>
        /// <param name="username">Pass the user's username</param>
        /// <returns>
        /// <paramref name="True"/> - user does not have admin rights.<br/>
        /// - or - <br/>
        /// <paramref name="False"/> - user has admin rights.
        /// </returns>
        public async Task<bool> NotAdmin(string? requestor)
        {
            if (requestor == null || requestor == "") return true;
            return !await _userRepository.IsUserAdmin(requestor);
        }

        /// <summary>
        /// Request the category repository to query the database<br/>
        ///  for the existence of a category named <paramref name="categoryName"/>.<br/>-----
        /// </summary>
        /// <param name="categoryName">The name of the category to check the existence of.</param>
        /// <returns>
        /// <paramref name="True"/> - category exists. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - category does not exist.
        /// </returns>
        public async Task<bool> CategoryExists(string categoryName)
        {
            return await _categoryRepository.CategoryExists(categoryName);
        }


        /// <summary>
        /// Request the category repository to query the database<br/>
        ///  for the existence of a category with the provided <paramref name="categoryId"/>.<br/>-----
        /// </summary>
        /// <param name="categoryId">The id of the category to check the existence of.</param>
        /// <returns>
        /// <paramref name="True"/> - category exists. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - category does not exist.
        /// </returns>
        public async Task<bool> CategoryExists(int categoryId)
        {
            return await _categoryRepository.CategoryExists(categoryId);
        }


        /// <summary>
        /// Request the category repository to query the database<br/>
        ///  for the existence of a category with the provided <paramref name="categoryId"/>.<br/>-----
        /// </summary>
        /// <param name="categoryId">The id of the category to check the existence of.</param>
        /// <returns>
        /// <paramref name="True"/> - category does not exist.
        /// - or - <br/>
        /// <paramref name="False"/> - category exists. <br/>
        /// </returns>
        public async Task<bool> CategoryDoesNotExist(int categoryId)
        {
            return !await CategoryExists(categoryId);
        }


        /// <summary>
        /// Check for null required fields in an object<br/>-----
        /// </summary>
        /// <param name="obj">Any of the predetermined types.</param>
        /// <returns>
        /// <paramref name="True"/> - required fields are null.<br/>
        ///  - or -<br/>
        /// <paramref name="True"/> - required fields are not null.
        /// </returns>
        public bool RequiredFieldsEmpty(dynamic obj)
        {
            if (obj.GetType() == typeof(UpdateCategoryDto))
                return (obj.NewCategoryName == null
                && obj.NewInfo == null
                && obj.NewImageFile == null);


            throw new NotImplementedException();
        }

    }
}