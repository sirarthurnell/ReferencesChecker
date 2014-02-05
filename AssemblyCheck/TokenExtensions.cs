using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCheck.Token
{
    public static class TokenExtensions
    {
        /// <summary>
        /// Translates a given token as array of bytes
        /// in a string.
        /// </summary>
        /// <param name="token">Token to translate.</param>
        /// <returns>String representation of the token.</returns>
        public static string TranslateToken(this byte[] token)
        {
            string tokenAsString = string.Empty;

            for (int i = 0; i <= token.GetUpperBound(0); i++)
            {
                tokenAsString += string.Format("{0:x2}", token[i]);
            }

            return tokenAsString;
        }
    }
}
