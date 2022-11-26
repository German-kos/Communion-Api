using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;

namespace API.BLL.Account
{
    public class AccountMappers : IAccountMappers
    {
        // Dependency Injections
        private readonly ITokenService _jwt;
        public AccountMappers(ITokenService jwt)
        {
            _jwt = jwt;
        }


        // Methods:


        public SignedInUserDto AccountMapper(AppUser user)
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


        public SignedInUserDto AccountMapper(AppUser user, bool remember)
        {
            var (id, username, name, profilePicture) = user;

            return new SignedInUserDto()
            {
                Id = id,
                Username = username,
                Name = name,
                ProfilePicture = profilePicture,
                Token = _jwt.CreateToken(user, remember),
                Remember = remember
            };
        }


        public ProfilePictureDto ProfilePictureMapper(ProfilePicture pfp)
        {
            // Deconstruction
            var (id, userId, publicId, url) = pfp;

            return new ProfilePictureDto
            {
                Id = id,
                UserId = userId,
                PublicId = publicId,
                Url = url
            };
        }


        public ProfileInformationDto ProfileInfoMapper(AppUser user)
        {
            var (username, name, dateOfBirth, bio, gender, country) = user;

            return new ProfileInformationDto
            {
                Username = username,
                Name = name,
                DateOfBirth = dateOfBirth,
                Bio = bio,
                Gender = gender,
                Country = country
            };
        }
    }
}