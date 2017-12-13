using System;
using System.Collections.Generic;
using System.Text;

namespace BookHeaven.Common.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToEuro(this decimal input)
            => $"{input:f2}€";
    }
}
