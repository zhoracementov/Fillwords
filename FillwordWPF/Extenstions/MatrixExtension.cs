﻿using FillwordWPF.Game;
using System;
using System.Collections.Generic;

namespace FillwordWPF.Extenstions
{
    internal static class MatrixExtension
    {
        public static IEnumerable<T> AsLinear<T>(this T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    yield return matrix[i, j];
                }
            }
        }

        public static int GetLinearArrayIndex<T>(this Point pnt, T[,] ar)
        {
            return pnt.GetLinearArrayIndex(ar.GetLength(0), ar.GetLength(1));
        }

        public static int GetLinearArrayIndex(this Point pnt, int w, int h)
        {
            if (pnt.Y >= h)
                throw new ArgumentException();

            return w * pnt.X + pnt.Y;
        }

        public static IEnumerable<Point> WhereAt<T>(this T[,] matrix, Func<T, bool> predicate)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    var item = matrix[i, j];

                    if (predicate(item))
                        yield return new Point(i, j);
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
