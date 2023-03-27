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

            max = Math.Min(max - 1, size * size);
            min = Math.Min(min, size * size);

            var rnd = new Random(Environment.TickCount);

            while (TryGetRandomFreePoint(table, out var currPoint, rnd, pnt => pnt == null))
            {
                var currNode = new Node<Point> { Value = currPoint };
                table[currPoint.X, currPoint.Y] = currNode;
                var nextLength = rnd.Next(min, max);

                bool IsCurrect = false;

                while (nextLength >= min && TryGetRandomAroundPoint(currPoint, out currPoint, rnd, pnt => table.IsInRange(pnt) && table.GetAt(pnt) == null))
                {
                    var nextNode = new Node<Point> { Value = currPoint, Previous = currNode };
                    currNode.Next = nextNode;
                    currNode = nextNode;

                    table[currPoint.X, currPoint.Y] = currNode;
                    nextLength--;
                    IsCurrect = true;
                }

                if (!IsCurrect)
                {
                    Console.WriteLine("Проблемная нода: [{0},{1}]", currPoint.X, currPoint.Y);
                    var around = GetPointsAround(currPoint, pnt =>
                    {
                        if (!table.IsInRange(pnt)) return false;
                        var node = table.GetAt(pnt);
                        return node != null && (node.Next == null || node.Previous == null);
                    });

                    if (around.Length == 0)
                    {
                        Console.WriteLine("ЭТА НОДА НЕ РАЗРЕШИМА!!!");
                        TryGetRandomAroundPoint(currPoint, out var killRndPoint, rnd, table.IsInRange);

                        table.SetAt(currPoint, null);

                        var destroyNode = table.GetAt(killRndPoint);

                        while (destroyNode.Previous != null)
                        {
                            destroyNode = destroyNode.Previous;
                        }

                        while (destroyNode != null)
                        {
                            table.SetAt(destroyNode.Value, null);
                            destroyNode = destroyNode.Next;
                        }
                    }
                    else
                    {
                        var aroundNode = table.GetAt(around.PickRandom(rnd));
                        if (aroundNode.Previous == null)
                        {
                            aroundNode.Previous = currNode;
                            currNode.Next = aroundNode;
                        }
                        else if (aroundNode.Next == null)
                        {
                            aroundNode.Next = currNode;
                            currNode.Previous = aroundNode;
                        }
                    }
                }

                var memory = new bool[size, size];
                var output = new string[size, size];
                var itemNumber = 0;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (memory[i, j])
                            continue;

                        var node = table[i, j];

                        if (node == null)
                            continue;

                        while (node.Previous != null)
                        {
                            node = node.Previous;
                        }

                        var nodeIndex = 0;
                        while (node != null)
                        {
                            var (x, y) = (node.Value.X, node.Value.Y);
                            memory[x, y] = true;
                            output[x, y] = $"[{itemNumber}|{nodeIndex++}]";

                            node = node.Next;
                        }
                        itemNumber++;
                    }
                }

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Console.Write(output[i, j] + "\t");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            }

            throw new NotImplementedException();
        }

        private Point[] GetPointsAround(Point point, Func<Point, bool> predicate)
        {
            var offset = new (int X, int Y)[]
            {
                (0, 1), (0, -1),
                (1, 0), (-1, 0),
            };

            return offset
                .Select(offsetCoord => new Point
                {
                    X = point.X + offsetCoord.X,
                    Y = point.Y + offsetCoord.Y
                })
                .Where(predicate)
                .ToArray();
        }

        private bool TryGetRandomFreePoint(Node<Point>[,] table, out Point output, Random random = null, Func<Node<Point>, bool> predicate = default)
        {
            var freePoints = table.WhereAt(predicate).ToArray();
            var tryPick = freePoints.Length > 0;
            output = tryPick ? freePoints.PickRandom(random) : null;
            return tryPick;
        }

        private bool TryGetRandomAroundPoint(Point point, out Point output, Random random = null, Func<Point, bool> predicate = default)
        {
            var nextPoints = GetPointsAround(point, predicate);
            var tryPick = nextPoints.Length > 0;
            output = tryPick ? nextPoints.PickRandom(random) : point;
            return tryPick;
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
