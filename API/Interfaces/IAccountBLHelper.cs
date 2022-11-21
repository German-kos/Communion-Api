using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;

namespace API.Interfaces
{
    /// <summary>
    /// Helper class for Account Business Logic Layer.
    /// </summary>
    public interface IAccountBLHelper
    {
        Task ProcessSignUp(SignUpFormDto signUpForm, List<Error> errors);
    }
}