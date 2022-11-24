using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public static class Rgx
    {
        /// <summary>
        /// Check username validity.
        /// </summary>
        public const string usernamePattern = @"^[a-zA-Z0-9@\-\.]*$";

        /// <summary>
        /// Check name validity.
        /// </summary>
        public const string namePattern = @"^[a-zA-Z@]*$";

        /// <summary>
        /// Check email validity.
        /// </summary>
        public const string emailPattern = @"^([a-z0-9_\.-]+\@[\da-z\.-]+\.[a-z\.]{2,6})$";

        /// <summary>
        /// Check if password has at least one digit.
        /// </summary>
        public const string pwHasDigit = "^(?=.*[0-9])";

        /// <summary>
        /// Check if password has at least one lower case character. 
        /// </summary>
        public const string pwHasLowerCase = "(?=.*[a-z])";

        /// <summary>
        /// Check if password has at least one upper case character.
        /// </summary>
        public const string pwHasUpperCase = "(?=.*[A-Z])";

        /// <summary>
        /// Check if password has at least one special character.
        /// </summary>
        public const string pwHasSpecialChar = "(?=.*[!@#$%^&-+=()])";

        /// <summary>
        /// Remove spaces from a string.
        /// </summary>
        public const string removeSpaces = @"\s+";
    }
}