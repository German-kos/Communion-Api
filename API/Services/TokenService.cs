using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Models;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        public SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user, bool remember)
        {
            // adding the claims to the token
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.Username)
            };

            // adding the credentials. need the key and the algorithm
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            DateTime tokenExpiration;
            if (remember) tokenExpiration = DateTime.Now.AddDays(7);
            else tokenExpiration = DateTime.Now.AddDays(1);

            // describe the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpiration,
                SigningCredentials = creds
            };

            // after creating the token, a token handler is needed
            var tokenHandler = new JwtSecurityTokenHandler();

            // creating the token itself
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // return the token
            return tokenHandler.WriteToken(token);
        }
    }
}