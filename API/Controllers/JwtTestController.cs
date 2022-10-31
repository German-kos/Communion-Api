using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class JwtTestController : BaseApiController
    {
        [HttpGet("test")]
        public async Task<ActionResult<string>> Test()
        {
            return "passed";
        }
    }
}