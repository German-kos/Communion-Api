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

            List<Error> errors = new List<Error>();

            var userValidation = UsernameValidations(username, errors);

            var emailValidation = EmailValidation(email, errors);

            PasswordValidations(password, errors);

            NameValidation(name, errors);


            await userValidation;

            await emailValidation;

            return errors;
        }


        // Private Methods:


        private async Task UsernameValidations(string username, List<Error> errors)
        {
            string un = "Username";

            // If a username is too long it may be malicious
            // and should not be processed by regex or be looked up in the database
            if (!IsLengthValid(username, 4, 12))
            {
                errors.Add(new Error(un, $"{un}s should be 4 - 12 characters long."));
                return;
            }

            var userExists = _repo.DoesUserExist(username);

            string rgxUsernamePattern = @"^[a-zA-Z0-9@\-\.]*$";
            if (!Regex.IsMatch(username, rgxUsernamePattern))
                errors.Add(new Error(un, $"{un}s must be alphanumerical."));

            if (await userExists)
                errors.Add(new Error("Username", Const.takenUsername));
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


        private void NameValidation(string name, List<Error> errors)
        {
            string n = "Name";

            // If a name is too long it may be malicious 
            // and should not be processed by regex
            if (!IsLengthValid(name, 1, 12))
            {
                errors.Add(new Error(n, $"{n}s should be 1 - 12 characters long."));
                return;
            }

            string rgxNamePattern = @"^[a-zA-Z@]*$";
            if (!Regex.IsMatch(name, rgxNamePattern))
                errors.Add(new Error(n, $"{n}s must only contain english letters."));
        }


        private async Task EmailValidation(string email, List<Error> errors)
        {
            string em = "Email";

            // If a email is too long it may be malicious
            // and should not be processed by regex or be looked up in the database
            var err = new Error(em, "Invalid email.");
            if (!IsLengthValid(email, 5, 64))
            {
                errors.Add(err);
                return;
            }

            var emailExists = _repo.DoesEmailExist(email);

            string rgxEmailPatten = @"^([a-z0-9_\.-]+\@[\da-z\.-]+\.[a-z\.]{2,6})$";
            if (!Regex.IsMatch(email, rgxEmailPatten))
                errors.Add(err);

            if (await emailExists)
                errors.Add(new Error(em, $"{em} is already in use."));
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