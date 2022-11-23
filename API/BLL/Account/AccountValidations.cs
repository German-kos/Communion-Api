using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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


        public async Task<List<Error>> ProcessSignUp(SignUpFormDto signUpForm)
        {
            // Deconstruction
            var (username, password, name, email) = signUpForm;

            var userExists = _repo.DoesUserExist(username);

            List<Error> errors = new List<Error>();

            PasswordValidations(password, errors);

            if (await userExists)
                errors.Add(new Error("Username", Const.takenUsername));

            return errors;
        }


        // Private Methods:


        private async Task UsernameValidations(string username, List<Error> errors)
        {
            string un = "Username";

            // If a username is too long it may be malicious
            // and should not be processed by regex
            if (!IsLengthValid(username, 4, 12))
            {
                errors.Add(new Error(un, $"{un}s should be 4 - 12 characters long."));
                return;
            }


        }


        /// <summary>
        /// Check if password meets requirements. If a requirement is not met, add an error to the list.
        /// </summary>
        /// <param name="password">The provided password.</param>
        /// <param name="errors">A list of errors to populate if a requirement isn't met</param>
        private void PasswordValidations(string password, List<Error> errors)
        {
            string pw = "Password";

            // If a password is too long it may be malicious 
            // and should not be processed by regex
            if (!IsLengthValid(password, 8, 20))
            {
                errors.Add(new Error(pw, $"{pw}s should be 8 - 20 characters long."));
                return;
            }

            string pws = "Passwords must contain at least one";

            var rgxPwChecks = RgxPasswordPatterns();

            int pwNoSpacesLenght = Regex.Replace(password, @"\s+", "").Length;

            foreach (var check in rgxPwChecks)
            {
                bool valid = Regex.IsMatch(password, check.Value);

                if (!valid)
                    switch (check.Key)
                    {
                        case "hasDigit":
                            errors.Add(new Error(pw, $"{pws} digit."));
                            break;
                        case "hasLowerCase":
                            errors.Add(new Error(pw, $"{pws} lower case character."));
                            break;
                        case "hasUpperCase":
                            errors.Add(new Error(pw, $"{pws} upper case character."));
                            break;
                        case "hasSpecialChar":
                            errors.Add(new Error(pw, $"{pws} special character."));
                            break;
                    }
            }

            if (pwNoSpacesLenght != password.Length)
                errors.Add(new Error(pw, $"{pw}s must not contain any spaces"));
        }


        /// <summary>
        /// A collection of regex patterns, and what they check. 
        /// </summary>
        /// <returns>Initialization of new Dictionary with the regex patterns.</returns>
        private Dictionary<string, string> RgxPasswordPatterns()
        {
            return new Dictionary<string, string>()
            {
                {"hasDigit", "^(?=.*[0-9])"},
                {"hasLowerCase", "(?=.*[a-z])"},
                {"hasUpperCase", "(?=.*[A-Z])"},
                {"hasSpecialChar", "(?=.*[@#$%^&-+=()])"}
            };
        }

        private bool IsLengthValid(string str, int minLength, int maxLength)
        {
            return str.Length <= maxLength && str.Length >= minLength;
        }
    }
}