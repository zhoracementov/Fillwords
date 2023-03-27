using DataLoaderConsoleTest.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLoaderConsoleTest
{
    internal static class IEnumerablePickExtention
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

        public static IEnumerable<T> GetAsLinear<T>(this T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    yield return matrix[i, j];
                }
            }
        }

        public static bool All<T>(this T[,] matrix, Func<T, bool> predicate)
        {
            return matrix.GetAsLinear().All(predicate);
        }

        public static IEnumerable<TOutput> Select<TSource, TOutput>(this TSource[,] matrix, Func<TSource, TOutput> func)
        {
            return matrix.GetAsLinear().Select(func);
        }

        public static IEnumerable<T> Where<T>(this T[,] matrix, Func<T, bool> predicate)
        {
            return matrix.GetAsLinear().Where(predicate);
        }

        public static IEnumerable<Point> WhereAt<T>(this T[,] matrix, Func<T, bool> predicate)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    var item = matrix[i, j];

                    if (predicate(item))
                        yield return new Point { X = i, Y = j };
                }
            }
        }

        public static T GetAt<T>(this T[,] matrix, Point point)
        {
            return matrix[point.X, point.Y];
        }

        public static void SetAt<T>(this T[,] matrix, Point point, T value)
        {
            matrix[point.X, point.Y] = value;
        }

        public static bool IsInRange<T>(this T[,] matrix, Point point)
        {
            return matrix.IsInRange(point.X, point.Y);
        }

        public static bool IsInRange<T>(this T[,] matrix, int x, int y)
        {
            return x >= 0 && y >= 0 && x < matrix.GetLength(0) && y < matrix.GetLength(1);
        }
    }
}
