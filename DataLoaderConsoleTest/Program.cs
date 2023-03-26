using DataLoaderConsoleTest.Data;
using DataLoaderConsoleTest.Load;
using System;
using System.Collections.Generic;

namespace DataLoaderConsoleTest
{
    internal class Program
    {
        private const string FileName = "data2.json";
        private const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        private static void Main()
        {
            var loader = new JsonWebLoader<Dictionary<string, WordInfo>>(URL, FileName);

            loader.WebDownloadDataAsync()
                .ContinueWith(x => loader.DeserializeDataFromFileAsync()
                .ContinueWith(x =>
                Print(new WordInfoDefinitionConverter(loader.Data).Convert()))
                .ContinueWith(x => Console.WriteLine(loader.IsLoaded)));

            Console.ReadKey();
        }

        private static void Print(IDictionary<string, WordInfo> data)
        {
            Console.WriteLine(string.Join(Environment.NewLine, data));
        }
    }
}
