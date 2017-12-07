using System;
using System.Collections.Generic;
using System.Text;

namespace BookHeaven.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsInsensitive(this string current, string input)
        {
            return current.ToLower().Contains(input.ToLower());
        }
    }
}
