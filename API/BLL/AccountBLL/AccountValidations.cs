using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.BLL.AccountBLL
{
    public class AccountValidations : IAccountValidations
    {
        // Dependency Injections
        private readonly IAccountRepository _repo;
        private readonly ITokenService _jwt;
        private readonly IAccountMappers _map;
        public AccountValidations(IAccountRepository repo, ITokenService jwt, IAccountMappers map)
        {
            _map = map;
            _jwt = jwt;
            _repo = repo;
        }


        // Methods:


        public bool UsernameRequestorMatch(string username, string? requestor)
        {
            return username.ToLower() == requestor?.ToLower();
        }
        public async Task<ConcurrentBag<Error>> ProcessSignUp(SignUpFormDto signUpForm)
        {
            // Deconstruction
            var (username, password, name, email) = signUpForm;

            // Threadsafe collection of errors
            ConcurrentBag<Error> errors = new ConcurrentBag<Error>();

            // Running the validations asynchronously on different threads
            List<Task> validations = new List<Task>()
            {
                {UsernameValidations(username, errors)},
                {EmailValidation(email, errors)},
                {Task.Run(() => PasswordValidations(password, errors))},
                {Task.Run(() => NameValidation(name, errors))}
            };

            // Wait for all the tasks to finish before returning the list
            await Task.WhenAll(validations);

            return errors;
        }


        public ActionResult<SignedInUserDto> ProcessSignUpResult(ActionResult<AppUser>? signUpResult)
        {
            // Deconstruction
            var (result, value) = (signUpResult?.Result, signUpResult?.Value);

            if (result != null)
                return result;

            if (value != null)
                return _map.AccountMapper(value);

            return new StatusCodeResult(500);
        }


        public async Task<ConcurrentBag<Error>> ProcessSignIn(SignInFormDto signInForm)
        {
            var (username, password) = signInForm;

            ConcurrentBag<Error> errors = new ConcurrentBag<Error>();

            bool usernameLengthValid = IsLengthValid(username, 4, 12);
            bool passwordLengthValid = IsLengthValid(password, 8, 20);

            // In case lengths don't match and they might be too short or long,
            // don't bother running the rest of the validations
            if (!usernameLengthValid)
                errors.Add(new Error("Username", "Invalid username."));
            if (!passwordLengthValid)
                errors.Add(new Error("Password", "Invalid password."));

            // If username is within expected range then it shouldn't be too long to process,
            // and might exist in the database
            if (usernameLengthValid)
            {
                bool userExists = await _repo.DoesUserExist(username);

                if (!userExists)
                    errors.Add(new Error("Username", $"{username} does not exist."));
            }

            return errors;
        }


        public async Task<SignedInUserDto?> ValidatePassword(SignInFormDto signInForm, ConcurrentBag<Error> errors)
        {
            // Deconstruction
            var (username, password, remember) = signInForm;

            // If the bag is already populated with errors, this method should not have been invoked.
            if (errors.Count() > 0)
                return null;

            var user = await _repo.GetUserByUsername(username);

            if (user == null)
            {
                errors.Add(new Error("Username", $"{username} does not exist."));
                return null;
            }

            bool passwordsMatch = PasswordsMatch(password, user.PasswordHash, user.PasswordSalt);
            if (passwordsMatch)
                return _map.AccountMapper(user, remember);

            errors.Add(new Error("Password", "Password and username do not match."));
            return null;
        }


        public async Task<ActionResult<SignedInUserDto>> ProcessAutoSignIn(AutoSignInFormDto autoSignInForm, string? requestor)
        {
            // Deconstruction
            var (username, remember) = autoSignInForm;

            if (username.ToLower() != requestor?.ToLower())
                return new UnauthorizedResult();

            var user = await _repo.GetUserByUsername(username);
            if (user != null)
                return _map.AccountMapper(user, remember);

            return new StatusCodeResult(500);
        }


        // Private Methods:


        /// <summary>
        /// A set of validations for the username, adds an error to the bag for each failed validation.
        /// </summary>
        /// <param name="username">The provided username.</param>
        /// <param name="errors">A bag of errors to populate if requirements aren't met.</param>
        private async Task UsernameValidations(string username, ConcurrentBag<Error> errors)
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

            if (!Regex.IsMatch(username, Rgx.usernamePattern))
                errors.Add(new Error(un, $"{un}s must be alphanumerical."));

            if (await userExists)
                errors.Add(new Error("Username", Const.takenUsername));
        }


        /// <summary>
        /// A set of validations for the password, adds an error to the bag for each failed validation.
        /// </summary>
        /// <param name="password">The provided password.</param>
        /// <param name="errors">A bag of errors to populate if requirements aren't met.</param>
        private void PasswordValidations(string password, ConcurrentBag<Error> errors)
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

            // A collection of regex patterns to check passwords.
            var rgxPwChecks = new Dictionary<string, string>()
            {
                {"hasDigit", Rgx.pwHasDigit},
                {"hasLowerCase", Rgx.pwHasLowerCase},
                {"hasUpperCase", Rgx.pwHasUpperCase},
                {"hasSpecialChar", Rgx.pwHasSpecialChar}
            };

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

            int pwNoSpacesLenght = Regex.Replace(password, Rgx.removeSpaces, "").Length;

            if (pwNoSpacesLenght != password.Length)
                errors.Add(new Error(pw, $"{pw}s must not contain any spaces"));
        }


        /// <summary>
        /// A set of validations for the password, adds an error to the bag when a validation fails.
        /// </summary>
        /// <param name="name">The provided name.</param>
        /// <param name="errors">A bag of errors to populate if requirements aren't met.</param>
        private void NameValidation(string name, ConcurrentBag<Error> errors)
        {
            string n = "Name";

            // If a name is too long it may be malicious 
            // and should not be processed by regex
            if (!IsLengthValid(name, 1, 12))
            {
                errors.Add(new Error(n, $"{n}s should be 1 - 12 characters long."));
                return;
            }

            if (!Regex.IsMatch(name, Rgx.namePattern))
                errors.Add(new Error(n, $"{n}s must only contain english letters."));
        }


        /// <summary>
        /// A set of validations for the password, adds an error to the bag when a validation fails.
        /// </summary>
        /// <param name="email">The provided email.</param>
        /// <param name="errors">A bag of errors to populate if requirements aren't met.</param>
        private async Task EmailValidation(string email, ConcurrentBag<Error> errors)
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

            if (!Regex.IsMatch(email, Rgx.emailPattern))
                errors.Add(err);

            if (await emailExists)
                errors.Add(new Error(em, $"{em} is already in use."));
        }


        /// <summary>
        /// String length validation for form fields.
        /// </summary>
        /// <param name="str">The provided string.</param>
        /// <param name="minLength">The required minimum length.</param>
        /// <param name="maxLength">The required maximum length.</param>
        /// <returns>
        /// <paramref name="True"/> - if the length is between the given range. <br/>
        /// - or - <br/>
        /// <paramref name="False"/> - if the length isn't between the given range.
        /// </returns>
        private bool IsLengthValid(string str, int minLength, int maxLength)
        {
            return str.Length <= maxLength && str.Length >= minLength;
        }




        /// <summary>
        /// Encrypt and check submitted password to the encrypted password.
        /// </summary>
        /// <param name="password">The client submitted password.</param>
        /// <param name="passwordHash">The encrypted password to compare to.</param>
        /// <param name="passwordSalt">The encryption key of the encrypted password.</param>
        /// <returns>
        /// <paramref name="True"/> - passwords match. <br/>
        /// - or - <br/> 
        /// <paramref name="False"/> - passwords don't match. 
        /// </returns>
        private bool PasswordsMatch(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != passwordHash[i])
                    return false;

            return true;
        }
    }
}