using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Data;
using api.Models;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IAccountRepository _accountRepository;
        public UsersController(DataContext context, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _context = context;
        }

        [HttpPost("get-user-by-username")]
        public async Task<ActionResult<API.Models.UserImage>> GetUserByUsername(UserByUsernameDto data)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Username.ToLower() == data.Username.ToLower());
            // user.ProfilePicture = await _context.ProfilePictures.OrderBy(img => img.Id).LastOrDefaultAsync(img => img.UserId == user.Id);
            return await _context.ProfilePictures.FirstOrDefaultAsync(img => img.UserId == user.Id);
            // return user;
            // return await _context.Users.Include(user => user.ProfilePicture).FirstOrDefaultAsync(user => user.Username.ToLower() == data.Username.ToLower());
        }
    }
}