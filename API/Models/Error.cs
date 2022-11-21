using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    /// <summary>
    /// A class for sending back errors
    /// </summary>
    public class Error
    {
        public string Field { get; set; } = null!;
        public string Message { get; set; } = null!;

        public Error(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
}