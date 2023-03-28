﻿using DataLoaderConsoleTest.Data;
using DataLoaderConsoleTest.Data.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLoaderConsoleTest.Table
{
    internal class FillwordTableRandomBuilder : FillwordTableBuilder
    {
        private readonly Random rnd = new Random(Environment.TickCount);
        private readonly Node<Point>[,] table;

        private readonly int min;
        private readonly int max;

        public FillwordTableRandomBuilder(IDictionary<string, WordInfo> words, int size, Difficulty difficulty)
            : base(words, size, difficulty)
        {
            this.table = new Node<Point>[size, size];

            var (min, max) = GetWordsLengthRange();

            this.min = Math.Min(min, size * size);
            this.max = Math.Min(max - 1, size * size);

            //TODO: create max len change by difficulty
        }

        public override FillwordTable Build()
        {
            while (TryGetRandomFreePoint(table, out var currPoint, rnd, pnt => pnt == null))
            {
                var currNode = new Node<Point> { Value = currPoint };
                table[currPoint.X, currPoint.Y] = currNode;
                var nextLength = rnd.Next(min, max);

                var IsCurrect = false;

                while (nextLength >= min && TryGetRandomAroundPoint(currPoint, out currPoint, rnd, pnt => table.IsInRange(pnt) && table.GetAt(pnt) == null))
                {
                    var nextNode = new Node<Point> { Value = currPoint, Previous = currNode };
                    currNode.Next = nextNode;
                    currNode = nextNode;

                    table[currPoint.X, currPoint.Y] = currNode;
                    nextLength--;

                    IsCurrect = true;
                }

                Print();

                if (IsCurrect)
                    continue;

                var around = GetPointsAround(currPoint, pnt =>
                {
                    if (!table.IsInRange(pnt)) return false;
                    var node = table.GetAt(pnt);
                    return node != null && (node.Next == null || node.Previous == null);
                });

                if (around.Length == 0)
                {
                    TryGetRandomAroundPoint(currPoint, out var bindedAroundPoint, rnd, table.IsInRange);

                    table.SetAt(currPoint, null);

                    var destroyNode = table.GetAt(bindedAroundPoint);
                    while (destroyNode.Previous != null)
                        destroyNode = destroyNode.Previous;

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

            var wordsTable = new FillwordTableItem[size, size];
            var wordsPlaces = table
                .Where(x => x.IsHead)
                .Select(x => GetPoints(x).ToArray())
                /*.ToArray()*/;

            foreach (var place in wordsPlaces)
            {
                var len = place.Length;
                var rndWord = words.Keys.PickRandom(rnd, x => x.Length == len);

                Console.WriteLine(rndWord);

                for (int i = 0; i < place.Length; i++)
                {
                    var point = place[i];

                    wordsTable[point.X, point.Y] = new FillwordTableItem
                    {
                        Point = point,
                        CurrentLetter = rndWord[i],
                        Word = rndWord,
                        Info = words[rndWord]
                    };
                }
            }

            return new FillwordTable(wordsTable);
        }

        private (int Min, int Max) GetWordsLengthRange()
        {
            var sorted = words.Keys.Select(x => x.Length).OrderBy(x => x).ToArray();
            return (sorted.First(), sorted.Last());
        }

        private static IEnumerable<Point> GetPoints(Node<Point> head)
        {
            var curr = head;
            while (curr != null)
            {
                yield return curr.Value;
                curr = curr.Next;
            }
        }

        private static bool TryGetRandomFreePoint(Node<Point>[,] table, out Point output, Random random = null, Func<Node<Point>, bool> predicate = default)
        {
            var freePoints = table.WhereAt(predicate).ToArray();
            var tryPick = freePoints.Length > 0;
            output = tryPick ? freePoints.PickRandom(random) : null;
            return tryPick;
        }

        private static bool TryGetRandomAroundPoint(Point point, out Point output, Random random = null, Func<Point, bool> predicate = default)
        {
            var nextPoints = GetPointsAround(point, predicate);
            var tryPick = nextPoints.Length > 0;
            output = tryPick ? nextPoints.PickRandom(random) : point;
            return tryPick;
        }

        private static Point[] GetPointsAround(Point point, Func<Point, bool> predicate)
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

        private void Print()
        {
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

                    while (!node.IsHead)
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
    }
}
