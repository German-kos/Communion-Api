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
using System.Reflection;

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


        public async Task<AppUser?> GetUserIncludePfp(string username)
        {
            return await _context.Users.Include(u => u.ProfilePicture).FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }


        public async Task<AppUser?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }


        public async Task<ActionResult<AppUser>> UpdateProfile(UpdateProfileFormDto updateProfileForm, List<PropertyInfo> fieldsToUpdate)
        {
            var (dateOfBirth, gender, country, bio) = updateProfileForm;
            List<Error> errors = new List<Error>();

            var user = await GetUserByUsername(updateProfileForm.Username);
            if (user == null)
                return new NotFoundObjectResult("User does not exist.");
            _context.Users.Attach(user);

            foreach (var field in fieldsToUpdate)
            {
                if (field.Name == "DateOfBirth")
                {
                    try
                    {
                        user.DateOfBirth = DateTime.Parse(dateOfBirth);
                        _context.Entry(user).Property(u => u.DateOfBirth).IsModified = true;
                    }
                    catch
                    {
                        errors.Add(new Error("DateOfBirth", "Invalid date format."));
                    }
                }
                else
                {

                    try
                    {
                        user.GetType().GetProperty(field.Name)?.SetValue(user, updateProfileForm.GetType().GetProperty(field.Name)?.GetValue(updateProfileForm));
                        _context.Entry(user).Property(field.Name).IsModified = true;
                    }
                    catch (InvalidCastException e)
                    {
                        errors.Add(new Error(field.Name, $"Couldn't update {field.Name}"));
                    }
                }
            }

            if (errors.Count > 0)
                return new ConflictObjectResult(errors);

            _context.Entry(user).State = EntityState.Modified;
            await SaveAllAsync();
            return user;
        }


        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}