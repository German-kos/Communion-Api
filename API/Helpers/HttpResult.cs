using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Helpers
{
    public class HttpResult : IHttpResultHelpers
    {
        //
        //
        // general generation of a result.
        // pass a number for the, and a string for the message 
        public ObjectResult GenerateResult(int statusCode, string message)
        {
            ObjectResult result = new ObjectResult(message);
            result.StatusCode = statusCode;
            return result;
        }
    }
}