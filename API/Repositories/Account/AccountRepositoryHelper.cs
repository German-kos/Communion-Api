using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;

namespace API.Repositories.Account
{
    /// <summary>
    /// Helper class for Account Data Access Layer.
    /// </summary>
    public class AccountRepositoryHelper
    {
        private readonly DataContext _context;
        public AccountRepositoryHelper(DataContext context)
        {
            _context = context;

        }
    }
}