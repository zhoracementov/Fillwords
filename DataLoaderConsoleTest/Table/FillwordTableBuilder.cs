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

            while (TryGetRandomFreePoint(table, out var currPoint, rnd, pnt => pnt == null))
            {
                var currNode = new Node<Point> { Value = currPoint };
                table[currPoint.X, currPoint.Y] = currNode;
                var nextLength = rnd.Next(min, max + 1);

                while (nextLength >= min && TryGetRandomAroundPoint(currPoint, out currPoint, rnd, pnt => IsInRange(pnt, table) && table[pnt.X, pnt.Y] == null))
                {
                    var nextNode = new Node<Point> { Value = currPoint, Previous = currNode };
                    currNode.Next = nextNode;
                    currNode = nextNode;

                    table[currPoint.X, currPoint.Y] = currNode;
                    nextLength--;
                }
            }

            //var memory = new bool[size, size];
            //var output = new string[size, size];
            //var itemNumber = 1;

            //for (int i = 0; i < size; i++)
            //{
            //    for (int j = 0; j < size; j++)
            //    {
            //        if (memory[i, j])
            //            continue;

            //        var node = table[i, j];
            //        while (node.Previous != null)
            //        {
            //            node = node.Previous;
            //        }

            //        var nodeIndex = 1;
            //        while (node != null)
            //        {
            //            var (x, y) = (node.Value.X, node.Value.Y);
            //            memory[x, y] = true;
            //            output[x, y] = $"{itemNumber} {nodeIndex++}";

            //            node = node.Next;
            //        }
            //        itemNumber++;
            //    }
            //}

            //for (int i = 0; i < size; i++)
            //{
            //    for (int j = 0; j < size; j++)
            //    {
            //        Console.Write(output[i, j] + "\t");
            //    }
            //    Console.WriteLine();
            //}

            throw new NotImplementedException();
        }

        private bool TryGetRandomFreePoint(Node<Point>[,] table, out Point output, Random random = null, Func<Node<Point>, bool> predicate = default)
        {
            var freePoints = table
                .WhereAt(predicate)
                .ToArray();

            var tryPick = freePoints.Length > 0;
            output = tryPick ? freePoints.PickRandom(random) : null;
            return tryPick;
        }

        private bool TryGetRandomAroundPoint(Point point, out Point output, Random random = null, Func<Point, bool> predicate = default)
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
                .Where(predicate)
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
