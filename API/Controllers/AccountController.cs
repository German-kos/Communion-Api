using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Models;
using API.DTOs;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenSerivce)
        {
            _tokenService = tokenSerivce;
            _context = context;
        }


        [HttpPost("signup")]
        public async Task<ActionResult<UserDto>> SignUp(RegisterDto registerDto)
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
            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("signin")]
        public async Task<ActionResult<SignedInUserDto>> SignIn(SignInDto signInDto)
        {
            // look up the username from sign in request in the database
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username.ToLower() == signInDto.Username.ToLower());
            // if the user does not exist, throw an unauthorized
            if (user == null) return Unauthorized("Invalid username or password");

            // if user exists, compare the passwords

            // taking the key from the user found in the database, and assigning it to the hmac
            using var hmac = new HMACSHA512(user.PasswordSalt);
            // taking the password from the sign in request and encrypting it with the assigned key
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signInDto.Password));
            // comparing the two encrypted passwords to check if they match
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid username or password");
            }
            // if the comparing loop passes, return the user found in the database in an appropriate format
            return new SignedInUserDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Token = "None" // this should be replaced with a JWT when implemented
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.Username.ToLower() == username.ToLower());
        }
    }
}