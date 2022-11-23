using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;

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
        /// <returns>A list of <paramref name="Error"/><br/>
        /// - or -<br/>
        /// An empty list if the form was valid.</returns>
        Task ProcessSignUp(SignUpFormDto signUpForm);
    }
}