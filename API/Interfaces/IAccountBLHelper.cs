using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Helper class for Account Business Logic Layer.
    /// </summary>
    public interface IAccountBLHelper
    {
        /// <summary>
        /// Request the data access layer to query the database for if a user exists.<br/>-----
        /// </summary>
        /// <param name="username">The username search by.</param>
        /// <returns>
        /// <paramref name="True"/> - if user exists.<br/>
        /// - or -<br/>
        /// <paramref name="False"/> - if user does not exist.<br/>
        /// </returns>
        Task<bool> UserExists(string username);
    }
}