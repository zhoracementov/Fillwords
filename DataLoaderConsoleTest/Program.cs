using DataLoaderConsoleTest.Data;
using DataLoaderConsoleTest.Load;
using DataLoaderConsoleTest.Table;
using System;
using System.Collections.Generic;

namespace DataLoaderConsoleTest
{
    internal class Program
    {
        private const string FileName = "data2.json";
        private const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        private static IDictionary<string, WordInfo> data;

        private static void Main()
        {
            var loader = new JsonWebDataLoader<Dictionary<string, WordInfo>>(URL, FileName);
            loader.LoadData().Wait();

            var table = new FillwordTableBuilder(loader.Data)
                .CreateRandomly(5, FillwordDifficulty.Medium);

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
