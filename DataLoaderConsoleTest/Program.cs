using DataLoaderConsoleTest.Data;
using DataLoaderConsoleTest.Load;
using DataLoaderConsoleTest.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataLoaderConsoleTest
{
    internal class Program
    {
        private const string FileName = "data.json";
        private const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        private static void Main()
        {
            var loader = new JsonWebDataLoader<WordsData>(URL, FileName);
            loader.LoadData().Wait();

            Console.WriteLine("Старт!");
            Console.ReadKey();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var table = new FillwordTableRandomBuilder(loader.Data, Difficulty.Medium).Build();

            stopWatch.Stop();

            Console.WriteLine("RunTime " + stopWatch.Elapsed.Ticks);

            Console.WriteLine();

            for (int i = 0; i < table.Size; i++)
            {
                for (int j = 0; j < table.Size; j++)
                {
                    Console.Write(table[i, j].CurrentLetter + " ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        private static IDictionary<string, WordInfo> DataConvert(IDictionary<string, WordInfo> data)
        {
            return new WordInfoDefinitionConverter(data).Convert();
        }

        private static void Print<T>(IEnumerable<T> data)
        {
            Console.WriteLine(string.Join<T>(Environment.NewLine, data));
        }
    }
}
