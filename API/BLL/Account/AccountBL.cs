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

namespace API.BLL.Account
{
    public class AccountBL : IAccountBL
    {
        // Dependency Injections
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountValidations _validate;
        private readonly ITokenService _jwt;
        public AccountBL(IAccountValidations validate, IAccountRepository accountRepository, ITokenService jwt)
        {
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

    }
}