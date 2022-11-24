using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    /// <summary>
    /// Account Business Logic Layer.
    /// </summary>
    public interface IAccountBL
    {
        /// <summary>
        /// Process a sign up request, if the request is valid, pass the a request to the repository to add the user to the database.
        /// </summary>
        /// <param name="signUpForm">The client submitted sign up form.</param>
        /// <returns>
        /// <paramref name="UserDto"/> remapped user. <br/>
        /// - or - <br/>
        /// <paramref name="HTTP"/> <paramref name="Response"/> what went wrong.
        /// </returns>
        Task<ActionResult<SignedInUserDto>> SignUp(SignUpFormDto signUpForm);

        /// <summary>
        /// Process a sign in request, if the request is valid, pass the a request to the repository to find the user from the database.
        /// </summary>
        /// <param name="signInForm">The client submitted sign in form.</param>
        /// <returns>
        /// <paramref name="UserDto"/> remapped user. <br/>
        /// - or - <br/>
        /// <paramref name="HTTP"/> <paramref name="Response"/> what went wrong.
        /// </returns>
        Task<ActionResult<SignedInUserDto>> SignIn(SignInFormDto signInForm);
    }
}