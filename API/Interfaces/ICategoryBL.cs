using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICategoryBL
    {
        Task<List<ForumCategory>> GetCategories();
        Task<List<ForumThread>> GetThreadsBySubCategoryId(int subCategoryId);
        Task<ForumCategory> AddCategory(); // should take in a dto for creating a category
    }
}