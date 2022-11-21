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

namespace API.BLL.Account
{
    public class AccountBL : IAccountBL
    {
        // Dependency Injections
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountBLHelper _helper;
        public AccountBL(IAccountBLHelper helper, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _helper = helper;
        }


        // Methods


        public async Task<ActionResult<UserDto>> SignUp(SignUpFormDto signUpForm)
        {
            // Deconstruction
            var (username, password, name, email) = signUpForm;

            // Error list for bad forms
            List<Error> errors = new List<Error>();

            await _helper.ProcessSignUp(signUpForm, errors);




        }
    }
}