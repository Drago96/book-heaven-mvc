using System;
using System.Collections.Generic;
using System.Text;

namespace BookHeaven.Common.Infrastructure.Constants
{
    public class ImageConstants
    {
        public static IEnumerable<string> SupportedImageContentTypes = new List<string>
        {
            "image/png",
            "image/jpeg",
            "image/gif",
            "image/bmp"
        };
    }
}
