using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using API.Interfaces;

namespace API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        // dependancy injections
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;

        }
        // save changes made to the database
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}