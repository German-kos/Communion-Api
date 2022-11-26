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
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        // Dependency Injections
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IImageService _imageService;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountBL _accountBL;
        public AccountController(DataContext context, ITokenService tokenSerivce, IImageService imageService, IAccountRepository accountRepository, IAccountBL accountBL)
        {
            _accountBL = accountBL;
            _accountRepository = accountRepository;
            _imageService = imageService;
            _tokenService = tokenSerivce;
            _context = context;
        }


        [HttpPost("signup")] // [POST] api/account/signup
        public async Task<ActionResult<SignedInUserDto>> SignUp([FromForm] SignUpFormDto signUpForm)
        {
            return await _accountBL.SignUp(signUpForm);
        }


        [HttpPost("signin")] // [POST] api/account/signin
        public async Task<ActionResult<SignedInUserDto>> SignIn([FromForm] SignInFormDto signInForm)
        {
            return await _accountBL.SignIn(signInForm);
        }


        [Authorize] // A bearer JWT is needed to auto sign-in
        [HttpPost("autosignin")] // [POST] api/account/autosignin
        public async Task<ActionResult<SignedInUserDto>> AutoSignIn([FromForm] AutoSignInFormDto autoSignInForm)
        {
            return await _accountBL.AutoSignIn(autoSignInForm, User.GetUsername());
        }


        [Authorize] // A bearer JWT is needed to upload a profile picture.
        [HttpPost("upload-pfp")] // [POST] api/account/upload-pfp
        public async Task<ActionResult<ProfilePictureDto>> UploadPfp([FromForm] UploadPfpFormDto uploadPfpForm)
        {
            return await _accountBL.UploadPfp(uploadPfpForm, User.GetUsername());

        }


        [Authorize] // A bearer JWT is needed to update a profile.
        [HttpPatch("edit-profile")] // [PATCH] api/account/edit-profile
        public async Task<ActionResult<ProfileInformationDto>> UpdateProfile([FromForm] UpdateProfileFormDto updateProfileForm)
        {
            return await _accountBL.UpdateProfile(updateProfileForm, User.GetUsername());
        }
    }
}