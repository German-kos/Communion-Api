using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using API.Interfaces;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;

        }
        public Task<AppUser> GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}