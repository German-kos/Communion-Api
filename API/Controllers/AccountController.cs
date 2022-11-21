using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Models;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IImageService _imageService;
        private readonly IAccountRepository _accountRepository;
        public AccountController(DataContext context, ITokenService tokenSerivce, IImageService imageService, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _imageService = imageService;
            _tokenService = tokenSerivce;
            _context = context;
        }


        [HttpPost("signup")]
        public async Task<ActionResult<UserDto>> SignUp(SignUpFormDto registerDto)
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
                Token = _tokenService.CreateToken(user, false)
            };
        }

        [HttpPost("signin")]
        public async Task<ActionResult<SignedInUserDto>> SignIn(SignInDto signInDto)
        {
            // look up the username from sign in request in the database
            var user = _accountRepository.GetUserByUsernameAsync(signInDto.Username).Result;

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

            string pfpUrl = "";
            // if (user.ProfilePicture.Count() > 0)
            pfpUrl = user.ProfilePicture.Url;

            return new SignedInUserDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Token = _tokenService.CreateToken(user, signInDto.Remember),
                ProfilePicture = pfpUrl,
                Remember = signInDto.Remember
            };
        }
        [Authorize]
        [HttpPost("autosignin")]
        public async Task<ActionResult<SignedInUserDto>> AutoSignIn(AutoSignInDto autoSignInUser)
        {
            var user = _accountRepository.GetUserByUsernameAsync(autoSignInUser.Username).Result;
            if (user == null) return StatusCode(500);

            string pfpUrl = "";
            // if (user.ProfilePicture.Count() > 0)
            pfpUrl = user.ProfilePicture.Url;

            return new SignedInUserDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Token = _tokenService.CreateToken(user, autoSignInUser.Remember),
                ProfilePicture = pfpUrl,
                Remember = autoSignInUser.Remember
            };
        }

        [Authorize]
        [HttpPost("upload-pfp")]
        public async Task<ActionResult<API.Models.ProfilePicture>> UploadPfp(IFormFile file)
        {
            var username = User.GetUsername();

            var user = await _accountRepository.GetUserByUsernameAsync(username);

            var result = await _imageService.UploadImageAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var image = new ProfilePicture
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            // user.ProfilePicture.Add(image);
            user.ProfilePicture = image;
            // user.ProfilePicture = 

            // _context.ProfilePictures.AddAsync(image);

            // user.ProfilePicture = image.Url; // check if works

            if (await _accountRepository.SaveAllAsync())
            {
                // return _context.ProfilePictures.SingleOrDefault(x => x.UserId == user.Id);
                // return new ImageDto
                // {
                //     Id = _context.Users..SingleOrDefault(x => x.PublicId == image.PublicId).Id,
                //     Url = image.Url
                // };
                return user.ProfilePicture;
            }
            return BadRequest("Failed to upload image");
        }

        // [Authorize]
        // [HttpPost("upload-pfp-test")]
        // public IFormFile UploadPfpTest(IFormFile file)
        // {
        //     return file;
        // }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.Username.ToLower() == username.ToLower());
        }

        [Authorize]
        [HttpPatch("edit-profile")]
        public async Task<ActionResult<AppUser>> EditProfile(EditProfileDto editProfileDto)
        {
            var username = User.GetUsername();

            var user = await _accountRepository.GetUserByUsernameAsync(username);
            user.Country = editProfileDto.Country;
            user.Gender = editProfileDto.Gender;
            user.Bio = editProfileDto.Bio;
            try
            {
                user.DateOfBirth = DateTime.Parse(editProfileDto.DateOfBirth);
            }
            catch when (editProfileDto.DateOfBirth == "")
            {
                user.DateOfBirth = null;
            }
            catch
            {
                Console.WriteLine("Invalid Request");
            }



            _context.Users.Attach(user);

            _context.Entry(user).Property(u => u.Country).IsModified = true;
            _context.Entry(user).Property(u => u.Country).IsModified = true;
            _context.Entry(user).Property(u => u.Gender).IsModified = true;
            _context.Entry(user).Property(u => u.DateOfBirth).IsModified = true;

            await _context.SaveChangesAsync();

            return await _accountRepository.GetUserByUsernameAsync(username);
        }
    }
}