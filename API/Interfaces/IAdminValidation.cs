using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAdminValidation
    {
        /// <summary>
        /// Check whether a user is an admin or not.
        /// </summary>
        /// <param name="username">The user to check admin rights for.</param>
        /// <returns>
        /// <paramref name="True"/> - User has admin rights.<br/>
        /// - or - <br/>
        /// <paramref name="False"/> - User does not have admin rights.
        /// </returns>
        Task<bool> IsUserAdmin(string? username);
    }
}