using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    /// <summary>
    /// This class handles validations for account requests.
    /// </summary>
    public interface IAccountValidations
    {
        /// <summary>
        /// Process the sign up form acquired from the client's request.
        /// </summary>
        /// <param name="signUpForm">The sign up form submitted by the client.</param>
        /// <returns>A bag of <paramref name="Error"/><br/>
        /// - or -<br/>
        /// An empty bag if the form was valid.</returns>
        Task<ConcurrentBag<Error>> ProcessSignUp(SignUpFormDto signUpForm);

        /// <summary>
        /// Process the sign up result, determine what to return.
        /// </summary>
        /// <param name="signUpResult">The sign up action result from the data access layer.</param>
        /// <returns>
        /// <paramref name="SignedInUserDto"/> of remapped user. <br/>
        /// - or - <br/>
        /// <paramref name="HTTP"/> <paramref name="Response"/> of error that occured.
        /// </returns>
        ActionResult<SignedInUserDto> ProcessSignUpResult(ActionResult<AppUser>? signUpResult);

        /// <summary>
        /// Process the sign in form acquired from the client's request.
        /// </summary>
        /// <param name="signInForm">The sign in form submitted by the client.</param>
        /// <returns>
        /// <returns>A bag of <paramref name="Error"/><br/>
        /// - or -<br/>
        /// An empty bag if the form was valid.</returns>
        /// </returns>
        Task<ConcurrentBag<Error>> ProcessSignIn(SignInFormDto signInForm);

        /// <summary>
        /// Find the user in the database, encrypt the password from the submitted sign in form, compare the database's result user encrypted password to the client provided password
        /// </summary>
        /// <param name="signInForm">The sign in form submitted by the client.</param>
        /// <param name="errors">The error bag to populate if there are any bad validations.</param>
        /// <returns>
        /// <paramref name="SignedInUserDto"/> of the user if the passwords match <br/>
        /// - or - <br/>
        /// <paramref name="null"/> if the password was incorrect, but an error was added to the error bag.
        /// </returns>
        Task<SignedInUserDto?> ValidatePassword(SignInFormDto signInForm, ConcurrentBag<Error> errors);

        /// <summary>
        /// Process the auto sign in form request from the client and determine whether to return a signed in user, or an unauthorized error.
        /// </summary>
        /// <param name="autoSignInForm">The client submitted auto sign in form.</param>
        /// <param name="requestor">The username of the requestor, extracted from the JWT.</param>
        /// <returns>
        /// <paramref name="SignedInUserDto"/> of the user from the request.<br/>
        /// - or - <br/>
        /// <paramref name="Unauthorized"/> - if the auto sign in form is not valid.
        /// </returns>
        Task<ActionResult<SignedInUserDto>> ProcessAutoSignIn(AutoSignInDto autoSignInForm, string? requestor);
    }
}