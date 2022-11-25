using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.BLL.Account
{
    public class AccountUpdates : IAccountUpdates
    {
        public Task<ActionResult<ProfileInformationDto>> ProcessUpdateProfile(UpdateProfileFormDto updateProfileForm)
        {
            var (username, dateOfBirth, country, gender, bio) = updateProfileForm;


        }
    }
}