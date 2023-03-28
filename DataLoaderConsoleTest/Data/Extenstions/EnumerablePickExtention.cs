﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLoaderConsoleTest.Data.Extenstions
{
    internal static class EnumerablePickExtention
    {
        private static readonly Random rnd = new Random(Environment.TickCount);

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count, Random random = null, Func<T, bool> predicate = default)
        {
            return source.ShakeAll(random, predicate).Take(count);
        }

        public static T PickRandom<T>(this IEnumerable<T> source, Random random = null, Func<T, bool> predicate = default)
        {
            var size = source.Count();
            random ??= rnd;

            T output;

            do
            {
                output = source.ElementAt(random.Next(size));
            } while (predicate != null && !predicate(output));
            return output;
        }

        public static IEnumerable<T> ShakeAll<T>(this IEnumerable<T> source, Random random = null, Func<T, bool> predicate = default)
        {
            return source.Where(predicate).OrderBy(x => (random ?? rnd).Next());
        }
    }
}
