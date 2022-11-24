using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.BLL.Account
{
    public class AccountValidations : IAccountValidations
    {
        // Dependency Injections
        private readonly IAccountRepository _repo;
        private readonly ITokenService _jwt;
        public AccountValidations(IAccountRepository repo, ITokenService jwt)
        {
            _jwt = jwt;
            _repo = repo;
        }


        // Methods:


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
                return AccountMapper(value);

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

            string rgxUsernamePattern = @"^[a-zA-Z0-9@\-\.]*$";
            if (!Regex.IsMatch(username, rgxUsernamePattern))
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
                {"hasDigit", "^(?=.*[0-9])"},
                {"hasLowerCase", "(?=.*[a-z])"},
                {"hasUpperCase", "(?=.*[A-Z])"},
                {"hasSpecialChar", "(?=.*[!@#$%^&-+=()])"}
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

            int pwNoSpacesLenght = Regex.Replace(password, @"\s+", "").Length;

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

            string rgxNamePattern = @"^[a-zA-Z@]*$";
            if (!Regex.IsMatch(name, rgxNamePattern))
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

            string rgxEmailPatten = @"^([a-z0-9_\.-]+\@[\da-z\.-]+\.[a-z\.]{2,6})$";
            if (!Regex.IsMatch(email, rgxEmailPatten))
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
        /// Remap <paramref name="AppUser"/> to <paramref name="SignedInUserDto"/>.
        /// </summary>
        /// <param name="user">The <paramref name="AppUser"/> to remap.</param>
        /// <returns><paramref name="SignedInUserDto"/> remapped user. </returns>
        private SignedInUserDto AccountMapper(AppUser user)
        {
            var (id, username, name, profilePicture) = user;

            return new SignedInUserDto()
            {
                Id = id,
                Username = username,
                Name = name,
                ProfilePicture = profilePicture,
                Token = _jwt.CreateToken(user, false)
            };
        }
    }
}