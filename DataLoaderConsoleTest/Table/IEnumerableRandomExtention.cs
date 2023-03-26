using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLoaderConsoleTest.Table
{
    public static class IEnumerableRandomExtention
    {
        private static readonly Random rnd = new Random(Environment.TickCount);

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count, Func<T, bool> predicate = default)
        {
            return source.ShakeAll(predicate).Take(count);
        }

        public static IEnumerable<T> ShakeAll<T>(this IEnumerable<T> source, Func<T, bool> predicate = default)
        {
            return source.Where(predicate).OrderBy(x => rnd.Next());
        }
    }
}
