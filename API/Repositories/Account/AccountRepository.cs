using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        // Dependency Injections
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

        public async Task<ActionResult<AppUser>> SignUp(AppUser newUser)
        {
            var addedUser = await _context.Users.AddAsync(newUser);
            if (!await SaveAllAsync())
                return new StatusCodeResult(500);
            return addedUser.Entity;
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


        public async Task<AppUser?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

        }


        public async Task<AppUser?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }


        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}