using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StenoCryptor.Web.Helpers
{
    public static class StringComparerHelper
    {
        /// <summary>
        /// Compares two strings.
        /// </summary>
        /// <param name="a">First string.</param>
        /// <param name="b">Second string.</param>
        /// <returns>Percent of match.</returns>
        public static string Compare(string a, string b)
        {
            int minLength = Math.Min(a.Length, b.Length);
            int match = 0;

            for (int i = 0; i < minLength; i++)
            {
                if (a[i] == b[i])
                    match++;
            }

            return ((float)match / (float)minLength).ToString("P");
        }
    }
}