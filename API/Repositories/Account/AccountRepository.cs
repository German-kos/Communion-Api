using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.DTOs;

namespace API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        private readonly IImageService _imageService;
        public AccountRepository(DataContext context, IImageService imageService)
        {
            _imageService = imageService;
            _context = context;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(u => u.ProfilePicture)
            .SingleOrDefaultAsync(user => user.Username.ToLower() == username.ToLower());
        }



        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        // new methods

        public async Task<bool> SignUp(AppUser newUser)
        {
            await _context.Users.AddAsync(newUser);
            return await SaveAllAsync();
        }


        public async Task<bool> DoesUserExist(string username)
        {
            return await _context.Users
            .AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }


        public async Task<bool> DoesEmailExist(string email)
        {
            return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }


        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}