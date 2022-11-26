using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ICategoryValidations
    {
        /// <summary>
        /// Request the category data access layer to check if a category with the given name exists already.
        /// </summary>
        /// <param name="categoryName">The category name to check for.</param>
        /// <returns>
        /// <paramref name="True"/> - A category by that name exists already.<br/>
        /// - or - <br/>
        /// <paramref name="False"/> - A category by that name does not exist.
        /// </returns>
        Task<bool> CategoryExists(string categoryName);
    }
}