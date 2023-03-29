using FillwordWPF.Infrastructure;
using FillwordWPF.Models;
using FillwordWPF.Services;

namespace FillwordWPF
{
    internal static class Initializer
    {
        public static WordsData WordsData { get; private set; }
        public static string FileName { get; private set; }
        public static string URL { get; private set; }

        public static void GetData()
        {
            URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";
            FileName = "data.json";

            var loader = new JsonWebDataLoader<WordsData>(URL, FileName);
            loader.LoadData().ContinueWith(x => WordsData = loader.Data);

            var w = new MainWindow();
            w.Show();
        }

        public static FillwordTable CreateTable(FillwordTableBuilder builder)
        {
            return builder.Build();
        }
    }
}
