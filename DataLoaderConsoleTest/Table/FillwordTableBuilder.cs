using DataLoaderConsoleTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLoaderConsoleTest.Table
{
    internal class FillwordTableBuilder
    {
        private readonly IDictionary<string, WordInfo> data;

        public FillwordTableBuilder(IDictionary<string, WordInfo> data)
        {
            this.data = data;
        }

        public FillwordTable CreateRandomly(int size, FillwordDifficulty difficulty)
        {
            var table = new Node<Point>[size, size];
            var (min, max) = GetWordsLengthRange();

            max = Math.Min(max, size * size);
            min = Math.Min(min, size * size);

            var rnd = new Random(Environment.TickCount);

            var countPoint = size * size;
            var countIteratedPoints = 0;

            throw new NotImplementedException();
        }

        private bool TryGetRandomFreePoint(Node<Point>[,] table, out Point output, Random random = null, Predicate<Point> predicate = default)
        {
            var freePoints = table
                .Where(x => predicate(x.Value))
                .ToArray();

            var tryPick = freePoints.Length > 0;
            output = tryPick ? freePoints.PickRandom(random).Value : null;
            return tryPick;
        }

        private bool TryGetRandomAroundPoint(Point point, out Point output, Random random = null, Predicate<Point> predicate = default)
        {
            var offset = new (int X, int Y)[]
            {
                (0, 1), (0, -1),
                (1, 0), (-1, 0),
            };

            var nextPoints = offset
                .Select(offsetCoord => new Point
                {
                    X = point.X + offsetCoord.X,
                    Y = point.Y + offsetCoord.Y
                })
                .Where(x => predicate(x))
                .ToArray();

            var tryPick = nextPoints.Length > 0;
            output = tryPick ? nextPoints.PickRandom(random) : null;
            return tryPick;
        }

        private bool IsInRange(Point point, Node<Point>[,] table)
        {
            return IsInRange(point.X, point.Y, table);
        }

        private bool IsInRange(int x, int y, Node<Point>[,] table)
        {
            return x >= 0 && y >= 0 && x < table.GetLength(0) && y < table.GetLength(1);
        }

        private (int Min, int Max) GetWordsLengthRange()
        {
            var sorted = data.OrderBy(x => x.Key.Length).ToArray();
            return (sorted.First().Key.Length, sorted.Last().Key.Length);
        }

        public FillwordTable Create(int size, params string[] keys)
        {
            throw new NotImplementedException();
        }
    }
}
