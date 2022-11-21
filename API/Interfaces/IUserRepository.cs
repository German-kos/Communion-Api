using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByUsername(string username);
        /// <summary>
        /// Check if the given user has admin rights
        /// </summary>
        /// <param name="username">Pass the user's username</param>
        /// <returns>
        /// <para>True - user has admin rights</para>
        /// <para>False - user does not have admin rights</para>
        /// </returns>
        Task<bool> IsUserAdmin(string username);
    }
}