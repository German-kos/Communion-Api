using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;

namespace API.BLL.CategoryBLL
{
    public class CategoryValidations : ICategoryValidations
    {
        // Dependency Injections
        private readonly ICategoryRepository _repo;
        public CategoryValidations(ICategoryRepository repo)
        {
            _repo = repo;
        }


        // Methods:


        public async Task<bool> CategoryExists(string categoryName)
        {
            return await _repo.CategoryExists(categoryName);
        }
    }
}