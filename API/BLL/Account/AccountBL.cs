using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;

namespace API.BLL.Account
{
    public class AccountBL : IAccountBL
    {
        // Dependency Injections
        private readonly IAccountRepository _accountRepository;
        private readonly AccountBLHelper _helper;
        public AccountBL(AccountBLHelper helper, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _helper = helper;
        }
    }
}