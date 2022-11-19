using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Helpers
{
    /// <summary>
    /// A helper class for creating HTTP responses
    /// </summary>
    public static class HttpResult
    {
        //
        //
        /// This method generates a result of the invoker's choice.
        /// pass a number for the, and a string for the message


        /// <summary>
        /// This method generates a 
        /// </summary>
        public static ObjectResult GenerateResponse(int statusCode, string? message)
        {
            ObjectResult result = new ObjectResult(message);
            result.StatusCode = statusCode;
            return result;
        }
        // Overload for just GenerateResult with just the status code
        public static ObjectResult GenerateResponse(int statusCode)
        {
            return GenerateResponse(statusCode, null);
        }
        //
        //
        // Generate result of '<x> does not exist'
        public static ObjectResult DoesNotExist(string name)
        {
            return GenerateResponse(409, $"\"{name}\" does not exist.");
        }
        // generate result of '<x> does not exist in <y>'
        public static ObjectResult DoesNotExist(string name, string insideOf)
        {
            return GenerateResponse(409, $"\"{name}\" does not exist in \"{insideOf}\".");
        }
        //
        //
        /// <summary>
        /// Generate a status code 204 - no content.
        /// </summary>
        /// <returns> 204 - No Content. </returns>
        public static ObjectResult NoContent()
        {
            return GenerateResponse(204, null);
        }

        /// <summary>
        /// Generate a status code 500 - generic error response.
        /// </summary>
        /// <returns> 500 - Something went wrong. </returns>
        public static ObjectResult InternalError()
        {
            return GenerateResponse(500, "Something went wrong.");
        }
    }
}