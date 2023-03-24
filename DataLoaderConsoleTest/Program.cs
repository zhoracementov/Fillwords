using System;

namespace DataLoaderConsoleTest
{
    internal class Program
    {
        private const string FileName = "data2.json";
        private const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        private static void Main()
        {
            var loader = new JsonRusWordsLoader(URL, FileName);

            loader.LoadDataAsync()
                .ContinueWith(x => loader.SerializeDataToFileAsync()).Wait();

            Console.WriteLine(string.Join(Environment.NewLine, loader.Data));

            Console.ReadKey();
        }
    }
}
