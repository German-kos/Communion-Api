using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            // checking if the given username already exists in the database
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Username = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                Name = registerDto.Name,
                Email = registerDto.Email,
                Bio = null,
                Interests = null,
                Country = null,
                Gender = null,
                ProfilePicture = null,
                DateOfBirth = null,
                RegistrationDate = DateTime.Now
            };
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.Username.ToLower() == username.ToLower());
        }
    }
}