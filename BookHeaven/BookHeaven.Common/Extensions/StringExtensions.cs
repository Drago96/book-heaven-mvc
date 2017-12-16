using System;
using BookHeaven.Common.Infrastructure.Constants;
using System.Linq;

namespace BookHeaven.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsInsensitive(this string current, string input)
        => current.ToLower().Contains(input.ToLower());
        

        public static bool IsValidImage(this string contentType)
            => ImageConstants.SupportedImageContentTypes.Contains(contentType);

        public static string GetPictureUrlOrDefault(this string picture, string defaultPath)
            => picture ?? defaultPath;

    }
}