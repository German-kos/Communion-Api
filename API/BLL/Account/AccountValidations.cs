using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Models;

namespace API.BLL.Account
{
    public class AccountValidations : IAccountValidations
    {
        // Dependency Injections
        private readonly IAccountRepository _repo;
        public AccountValidations(IAccountRepository repo)
        {
            _repo = repo;
        }


        // Methods:


        public async Task<IEnumerable<Error>> ProcessSignUp(SignUpFormDto signUpForm)
        {
            // Deconstruction
            var (username, password, name, email) = signUpForm;

            var userExists = _repo.DoesUserExist(username);

            bool passwordTooLong = IsPasswordTooLong(password);

            if (await userExists)
                yield return new Error("Username", Const.takenUsername);

            if (passwordTooLong)
                yield return new Error("Password", "Passwords should be 8 - 32 characters long.");
            else yield return IsPasswordValid(password);
        }


        // Private Methods:


        private bool IsPasswordTooLong(string password)
        {
            return password.Length > 32;
        }

        private IEnumerable<Error> IsPasswordValid(string password)
        {
            string pw = "Password";
            string pws = "Passwords";

            if (password.Trim().Length != password.Length)
                yield return new Error(pw, $"{pws} shouldn't start or end with spaces.");

            if (password.)
        }

    }
}