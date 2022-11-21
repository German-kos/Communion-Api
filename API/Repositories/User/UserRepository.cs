using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> IsUserAdmin(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()
            && u.IsAdmin);
        }

    }
}