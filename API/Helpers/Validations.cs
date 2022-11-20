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
        public Validations(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //
        // Methods:
        //


        /// <summary>
        /// Request the user repository to query whether or not the given user
        /// has admin rights.<br/>-----
        /// </summary>
        /// <param name="username">Pass the user's username</param>
        /// <returns>
        /// False - user has admin rights.<br/>
        /// - or - <br/>
        /// True - user does not have admin rights.
        /// </returns>
        public async Task<bool> NotAdmin(string username)
        {
            if (username == null || username == "") return true;
            return !await _userRepository.IsUserAdmin(username);
        }
    }
}