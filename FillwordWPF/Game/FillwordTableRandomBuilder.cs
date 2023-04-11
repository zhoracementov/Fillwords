using FillwordWPF.Game.Extenstions;
using FillwordWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FillwordWPF.Game
{
    internal class FillwordTableRandomBuilder : IFillwordTableBuilder
    {
        public const int MinWordLength = 2;

        private static readonly Random rnd = new Random(Environment.TickCount);

        private readonly WordsData words;
        private readonly int minWordLength;

        private readonly int min;
        private readonly int max;
        private readonly int size;

        public FillwordTableRandomBuilder(WordsData words, int size)
        {
            this.size = size;

            var (min, max) = GetWordsLengthRange();

            this.min = Math.Min(Math.Max(minWordLength, min), size * size);
            this.max = Math.Min(max - 1, size * size / 2);

            //TODO: create max len change by difficulty
        }

        public FillwordItem[,] Build()
        {
            var table = new Node<Point>[size, size];
            while (TryGetRandomPoint(table, out var currPoint, rnd, pnt => pnt == null))
            {
                var currNode = new Node<Point> { Value = currPoint };

                table[currPoint.X, currPoint.Y] = currNode;
                var nextLength = rnd.Next(min, max);
                var itLength = nextLength;

                var invertList = new List<Node<Point>>(itLength) { currNode };

                //Console.WriteLine("{0}, {1}", currPoint, nextLength);

                while (nextLength > 0 && TryGetRandomAroundPoint(currPoint, out currPoint, rnd, pnt => table.IsInRange(pnt) && table.GetAt(pnt) == null))
                {
                    var nextNode = new Node<Point> { Value = currPoint, Previous = currNode };
                    currNode.Next = nextNode;
                    currNode = nextNode;

                    table[currPoint.X, currPoint.Y] = currNode;
                    nextLength--;

                    invertList.Add(nextNode);

                    //Console.WriteLine("{0}, {1}", currPoint, nextLength);
                }

                //Print();

                if (itLength - nextLength >= min)
                    continue;

                foreach (var rev in invertList)
                {
                    table.SetAt(rev.Value, null);
                }

                foreach (var rev in invertList)
                {
                    var connect = GetPointsAround(rev.Value, pnt => table.IsInRange(pnt) && table.GetAt(pnt) != null);

                    if (connect.Length > 0)
                    {
                        var rndPick = connect.PickRandom(rnd);
                        var connNode = table.GetAt(rndPick);

                        while (connNode.Previous != null)
                        {
                            connNode = connNode.Previous;
                        }

                        while (connNode != null)
                        {
                            table.SetAt(connNode.Value, null);
                            connNode = connNode.Next;
                        }

                        //Print();
                        break;
                    }
                }
            }


            //Print();

            var wordsTable = new FillwordItem[size, size];
            var wordsPlaces = table
                .AsLinear()
                .Where(x => x.IsHead)
                .Select(x => x.TraceAllValues().ToArray())
                /*.ToArray()*/;

            var selectedWords = new List<string>();

            foreach (var place in wordsPlaces)
            {
                var len = place.Length;

                if (!words.Keys.TryPickRandom(out var rndWord, rnd, x => x.Length == len && !selectedWords.Contains(x)))
                {
                    throw new ArgumentException();
                }

                selectedWords.Add(rndWord);

                //Console.WriteLine(rndWord);

                for (int i = 0; i < place.Length; i++)
                {
                    var point = place[i];

                    wordsTable[point.X, point.Y] = new FillwordItem
                    {
                        Point = point,
                        CurrentLetter = rndWord[i],
                        Word = rndWord,
                        Info = words[rndWord]
                    };
                }
            }

            return wordsTable;
        }

        private (int Min, int Max) GetWordsLengthRange()
        {
            var sorted = words.Keys.Select(x => x.Length).OrderBy(x => x).ToArray();
            return (sorted.First(), sorted.Last());
        }

        private static bool TryGetRandomPoint(Node<Point>[,] table, out Point output, Random random = null, Func<Node<Point>, bool> predicate = default)
        {
            var freePoints = table.WhereAt(predicate).ToArray();
            var tryPick = freePoints.Length > 0;
            output = tryPick ? freePoints.PickRandom(random) : null;
            return tryPick;
        }

        private static bool TryGetRandomAroundPoint(Point point, out Point output, Random random = null, Func<Point, bool> predicate = default)
        {
            return GetPointsAround(point, predicate).TryPickRandom(out output, random, predicate);
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

        //private void Print()
        //{
        //    var memory = new bool[size, size];
        //    var output = new string[size, size];
        //    var itemNumber = 0;

        //    for (int i = 0; i < size; i++)
        //    {
        //        for (int j = 0; j < size; j++)
        //        {
        //            if (memory[i, j])
        //                continue;

        //            var node = table[i, j];

        //            if (node == null)
        //                continue;

        //            while (!node.IsHead)
        //            {
        //                node = node.Previous;
        //            }

        //            var nodeIndex = 0;
        //            while (node != null)
        //            {
        //                var (x, y) = (node.Value.X, node.Value.Y);
        //                memory[x, y] = true;
        //                output[x, y] = $"[{itemNumber}|{nodeIndex++}]";

        //                node = node.Next;
        //            }
        //            itemNumber++;
        //        }
        //    }

        //    Console.WriteLine("-----------------------------------------------------------------------------------------------------");
        //    for (int i = 0; i < size; i++)
        //    {
        //        for (int j = 0; j < size; j++)
        //        {
        //            Console.Write(output[i, j] + "\t");
        //        }
        //        Console.WriteLine();
        //    }

        //    Console.WriteLine("-----------------------------------------------------------------------------------------------------");
        //}
    }
}
