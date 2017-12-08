using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookHeaven.Common.Infrastructure.Constants;

namespace BookHeaven.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsInsensitive(this string current, string input)
        {
            return current.ToLower().Contains(input.ToLower());
        }

        public static bool IsValidImage(this string contentType)
            => ImageConstants.SupportedImageContentTypes.Contains(contentType);
    }
}
