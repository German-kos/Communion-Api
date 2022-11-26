using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Mvc.ControllerBase;
using API.Models;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Concurrent;

namespace API.BLL.Account
{
    public class AccountBL : IAccountBL
    {
        // Dependency Injections
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountValidations _validate;
        private readonly ITokenService _jwt;
        private readonly IAccountImageService _img;
        private readonly IAccountUpdates _update;
        public AccountBL(IAccountValidations validate, IAccountRepository accountRepository, ITokenService jwt, IAccountImageService img, IAccountUpdates update)
        {
            _update = update;
            _img = img;
            _jwt = jwt;
            _validate = validate;
            _accountRepository = accountRepository;
        }


        // Methods


        public async Task<ActionResult<SignedInUserDto>> SignUp(SignUpFormDto signUpForm)
        {
            // Deconstruction
            var (username, password, name, email) = signUpForm;

            // Error list for bad forms
            List<Error> errors = new List<Error>(await _validate.ProcessSignUp(signUpForm));

            if (errors.Count() > 0)
                return new BadRequestObjectResult(errors);

            // Create a new user to add to the database
            using var hmac = new HMACSHA512();
            var newUser = new AppUser
            {
                Username = username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key,
                Name = name,
                Email = email,
                RegistrationDate = DateTime.Now
            };

            var signUpResult = await _accountRepository.SignUp(newUser);

            return _validate.ProcessSignUpResult(signUpResult);
        }


        public async Task<ActionResult<SignedInUserDto>> SignIn(SignInFormDto signInForm)
        {
            var (username, password, remember) = signInForm;
            ConcurrentBag<Error> errors = new ConcurrentBag<Error>(await _validate.ProcessSignIn(signInForm));

            if (errors.Count() > 0)
                return new BadRequestObjectResult(errors);

            var validationResult = await _validate.ValidatePassword(signInForm, errors);

            if (errors.Count() > 0 || validationResult == null)
                return new BadRequestObjectResult(errors);

            return validationResult;
        }


        public async Task<ActionResult<SignedInUserDto>> AutoSignIn(AutoSignInFormDto autoSignInForm, string? requestor)
        {
            return await _validate.ProcessAutoSignIn(autoSignInForm, requestor);
        }


        public async Task<ActionResult<ProfilePictureDto>> UploadPfp(UploadPfpFormDto uploadPfpForm, string? requestor)
        {
            bool validUserRequest = _validate.UsernameRequestorMatch(uploadPfpForm.Username, requestor);
            if (!validUserRequest)
                return new UnauthorizedResult();

            return await _img.UploadPfp(uploadPfpForm);
        }

        public async Task<ActionResult<AppUser>> UpdateProfile(UpdateProfileFormDto updateProfileForm, string? requestor)
        {
            bool validUserRequest = _validate.UsernameRequestorMatch(updateProfileForm.Username, requestor);
            if (!validUserRequest)
                return new UnauthorizedResult();

            return await _update.ProcessUpdateProfile(updateProfileForm);
        }
    }
}