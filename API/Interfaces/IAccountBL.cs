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

        /// <summary>
        /// Process the auto sign in request, if the request is valid, return the user from the database corresponding to the reqestor.
        /// </summary>
        /// <param name="autoSignInForm">The client subtmitted auto sign in form.</param>
        /// <param name="requestor">The username of the requestor, extracted from the JWT.</param>
        /// <returns>
        /// <paramref name="SignedInUserDto"/> of the user from the request.<br/>
        /// - or - <br/>
        /// <paramref name="Unauthorized"/> - if the auto sign in form is not valid.
        /// </returns>
        Task<ActionResult<SignedInUserDto>> AutoSignIn(AutoSignInFormDto autoSignInForm, string? requestor);

        /// <summary>
        /// Process the upload pfp request, if the request is valid upload the picture to the database, add it to the corresponding requestor.
        /// </summary>
        /// <param name="uploadPfpForm">The client submitted upload pfp form.</param>
        /// <param name="requestor">The username of the requestor, extracted from the JWT.</param>
        /// <returns>
        /// <paramref name="ProfilePicture"/> of the added profile picture.
        /// </returns>
        Task<ActionResult<ProfilePictureDto>> UploadPfp(UploadPfpFormDto uploadPfpForm, string? requestor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editProfileForm"></param>
        /// <param name="requestor"></param>
        /// <returns></returns>
        Task<ActionResult<ProfileInformationDto>> UpdateProfile(UpdateProfileFormDto editProfileForm, string requestor);
    }
}