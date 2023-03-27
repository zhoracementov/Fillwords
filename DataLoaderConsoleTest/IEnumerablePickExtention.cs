using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLoaderConsoleTest
{
    public static class IEnumerablePickExtention
    {
        private static readonly Random rnd = new Random(Environment.TickCount);

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count, Random random = null, Func<T, bool> predicate = default)
        {
            return source.ShakeAll(random, predicate).Take(count);
        }

        public static T PickRandom<T>(this IEnumerable<T> source, Random random = null, Func<T, bool> predicate = default)
        {
            var count = source.Count();
            T output;

            do
            {
                output = source.ElementAt((random ?? rnd).Next(count));
            } while (!predicate(output));
            return output;
        }

        public static IEnumerable<T> ShakeAll<T>(this IEnumerable<T> source, Random random = null, Func<T, bool> predicate = default)
        {
            return source.Where(predicate).OrderBy(x => (random ?? rnd).Next());
        }

        public static bool All<T>(this T[,] matrix, Predicate<T> predicate)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (!predicate(matrix[i, j]))
                        return false;
                }
            }
            return true;
        }

        public static IEnumerable<T> Where<T>(this T[,] matrix, Predicate<T> predicate)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (predicate(matrix[i, j]))
                        yield return matrix[i, j];
                }
            }
        }
    }
}
