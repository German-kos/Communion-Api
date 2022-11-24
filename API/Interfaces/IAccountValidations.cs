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
    }
}