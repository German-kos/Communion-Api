using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;

namespace API.Helpers
{
    public class AdminValidation : IAdminValidation
    {
        // Dependency Injections
        private readonly IAccountRepository _repo;
        public AdminValidation(IAccountRepository repo)
        {
            _repo = repo;
        }


        // Methods:


        public async Task<bool> IsUserAdmin(string? username)
        {
            if (username == null)
                return false;
            var user = await _repo.GetUserByUsername(username);
            if (user != null && user.IsAdmin)
                return true;
            return false;
        }
    }
}