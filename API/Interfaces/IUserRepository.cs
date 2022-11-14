using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByUsername(string username);
    }
}