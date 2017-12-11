using System;
using System.Collections.Generic;
using System.Text;

namespace BookHeaven.Common.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ConvertToBase64String(this byte[] array)
            => array != null ? Convert.ToBase64String(array) : null;
    }
}
