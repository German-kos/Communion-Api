using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Models;
using API.Repositories.Account;

namespace API.BLL.Account
{

    public class AccountBLHelper : IAccountBLHelper
    {
        // Dependency Injections
        private readonly IAccountRepositoryHelper _repo;
        public AccountBLHelper(IAccountRepositoryHelper repo)
        {
            _repo = repo;
        }


        // Methods


        public async Task ProcessSignUp(SignUpFormDto signUpForm, List<Error> errors)
        {
            // Deconstruction
            var (username, password, name, email) = signUpForm;

            var userExists = _repo.UserExists(username);



            if (await userExists)
                errors.Add(new Error("Username", Const.takenUsername));
        }



    }
}