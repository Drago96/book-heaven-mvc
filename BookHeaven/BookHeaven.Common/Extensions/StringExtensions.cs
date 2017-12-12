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

        public static byte[] ConvertToByteArray(this string input)
            => input != null ? Convert.FromBase64String(input) : null;

        public static string GetBase64PictureOrDefault(this string picture, string defaultPath)
            => picture == null ? defaultPath : "data:image / png; base64," + picture;

    }
}