using DataLoaderConsoleTest.Data;
using DataLoaderConsoleTest.Load;
using DataLoaderConsoleTest.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLoaderConsoleTest
{
    internal class Program
    {
        private const string FileName = "data2.json";
        private const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        private static void Main()
        {
            var loader = new JsonWebDataLoader<Dictionary<string, WordInfo>>(URL, FileName);
            loader.LoadData().Wait();

            var (min, max) = GetWordsLengthRange(loader.Data.Keys);

            var builder = new FillwordTableRandomBuilder(Difficulty.Medium, 8, min, max).Build();

            Console.ReadKey();
        }

        private static (int Min, int Max) GetWordsLengthRange(IEnumerable<string> source)
        {
            var sorted = source.Select(x => x.Length).OrderBy(x => x).ToArray();
            return (sorted.First(), sorted.Last());
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
