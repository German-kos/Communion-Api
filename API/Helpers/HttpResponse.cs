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
    public static class HttpResponse
    {
        /// <summary>
        /// Generates any HTTP Response with recieved status code and message.<br/>-----
        /// </summary>
        /// <param name="statusCode">The HTTP Response status code.</param>
        /// <param name="message">The message that would be attached.</param>
        /// <returns>The generated [HTTP Response].</returns>
        public static ObjectResult GenerateResponse(int statusCode, string? message)
        {
            ObjectResult result = new ObjectResult(message);
            result.StatusCode = statusCode;
            return result;
        }


        /// <summary>
        /// Generates any HTTP Response with recieved status code and no message.<br/>-----
        /// </summary>
        /// <param name="statusCode">The HTTP Response status code.</param>
        /// <returns>The generated <paramref name="HTTP"/> <paramref name="Response"/>.</returns>
        public static ObjectResult GenerateResponse(int statusCode)
        {
            return GenerateResponse(statusCode, null);
        }


        /// <summary>
        /// Generate a  ` 404 - <paramref name="name"/> does not exist.` HTTP Response.<br/>-----
        /// </summary>
        /// <param name="item">The name of the item to display at the beginning.</param>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> 404 - <paramref name="name"/> does not exist.</returns>
        public static ObjectResult DoesNotExist(string item)
        {
            return GenerateResponse(404, $"\"{item}\" does not exist.");
        }


        /// <summary>
        /// Generate a ` 404 - <paramref name="name"/> does not exist in <paramref name="insideOf"/>.` HTTP Response.<br/>-----
        /// </summary>
        /// <param name="item1">The name of the item to display at the beginning.</param>
        /// <param name="item2">The name of the item that does not contain the first item.</param>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> 404 - <paramref name="name"/> does not exist in <paramref name="insideOf"/>.</returns>
        public static ObjectResult DoesNotExist(string item1, string item2)
        {
            return GenerateResponse(404, $"\"{item1}\" does not exist in \"{item2}\".");
        }


        /// <summary>
        /// Generate a ` 204 - No Content.` HTTP Response.<br/>-----
        /// </summary>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> 204 - No Content.</returns>
        public static ObjectResult NoContent()
        {
            return GenerateResponse(204, null);
        }


        /// <summary>
        /// Generate ` 500 - generic error response.` HTTP Response.<br/>-----
        /// </summary>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> 500 - Something went wrong. </returns>
        public static ObjectResult InternalError()
        {
            return GenerateResponse(500, "Something went wrong.");
        }


        /// <summary>
        /// Generate ` 401 - No permissions to execute this action.`<paramref name="HTTP"/> <paramref name="Response"/>.<br/>-----
        /// </summary>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> 401 - No permissions to execute this action.</returns>
        public static ObjectResult Unauthorized()
        {
            return GenerateResponse(401, "No permission to execute this action.");
        }


        /// <summary>
        /// Generate ` 409 - <paramref name="item"/> already exists.` HTTP Response. <br/>-----
        /// </summary>
        /// <param name="item">The name of the item to display at the beginning.</param>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> 409 - <paramref name="item"/> already exists.</returns>
        public static ObjectResult AlreadyExists(string item)
        {
            return GenerateResponse(409, $"\"{item}\" already exists.");
        }


        /// <summary>
        /// Generate ` 409 - <paramref name="item1"/> already exists in <paramref name="item2"/>.` HTTP Response. <br/>-----
        /// </summary>
        /// <param name="item1">The name of the item to display at the beginning.</param>
        /// <param name="item2">The name of the item that contains the first item.</param>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> 409 - <paramref name="item1"/> already exists in <paramref name="item2"/>.</returns>
        public static ObjectResult AlreadyExists(string item1, string item2)
        {
            return GenerateResponse(409, $"\"{item1}\" already exists in \"{item2}\".");
        }


        /// <summary>
        /// Generate ` 202 - <paramref name="item"/> has been deleted successfully.` HTTP Response.<br/>-----
        /// </summary>
        /// <param name="item">The name of the item to display at the beginning.</param>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> 202 - <paramref name="item"/> has been deleted successfully.</returns>
        public static ObjectResult DeletionSuccessful(string item)
        {
            return GenerateResponse(202, $"{item} has been deleted successfully.");
        }


        /// <summary>
        ///  Generate ` 304 - Not modified.` HTTP Response.<br/>-----
        /// </summary>
        /// <returns><paramref name="HTTP"/> <paramref name="Response"/> 304 - Not modified.` HTTP Response.</returns>
        public static ObjectResult NotModified()
        {
            return GenerateResponse(304, "Not modified.");
        }
    }
}