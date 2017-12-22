using System.Collections.Generic;
using System.Linq;

namespace BookHeaven.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool NullOrEmpty<T>(this IEnumerable<T> collection)
            => collection == null || !collection.Any();
    }
}