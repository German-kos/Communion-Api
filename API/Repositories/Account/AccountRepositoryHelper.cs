using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Account
{
    public class AccountRepositoryHelper : IAccountRepositoryHelper
    {
        // Dependency Injections
        private readonly DataContext _context;
        public AccountRepositoryHelper(DataContext context)
        {
            _context = context;
        }


        // Methods


        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }
    }
}