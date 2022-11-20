using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;

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
        public async Task<bool> NotAdmin(string requestor)
        {
            if (requestor == null || requestor == "") return true;
            return !await _userRepository.IsUserAdmin(requestor);
        }

        /// <summary>
        /// Request the category repository to query the database<br/>
        ///  for the existence of a category named <paramref name="categoryName"/>.<br/>-----
        /// </summary>
        /// <param name="categoryName">The name of the category to check for it's existence.</param>
        /// <returns>
        /// <paramref name="True"/> - category exists. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - category does not exist.
        /// </returns>
        public async Task<bool> CategoryExists(string categoryName)
        {
            return await _categoryRepository.CategoryExists(categoryName);
        }
    }
}