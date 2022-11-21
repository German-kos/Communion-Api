using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Repositories.Account;

namespace API.BLL.Account
{

    public class AccountBLHelper : IAccountBLHelper
    {
        // Dependency Injections
        private readonly AccountRepositoryHelper _repositoryHelper;
        public AccountBLHelper(AccountRepositoryHelper repositoryHelper)
        {
            _repositoryHelper = repositoryHelper;
        }


        // Methods


        public async Task<bool> UserExists(string username)
        {
            return await _repositoryHelper.DoesUserExist(username);
        }
    }
}