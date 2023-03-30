using FillwordWPF.Infrastructure;
using FillwordWPF.Services;

namespace FillwordWPF.Models
{
    internal class FillwordGenerator
    {
        private const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        public Fillword Fillword { get; }

        public FillwordGenerator()
        {
            WordsData wordsData = null;
            var gameSettings = new GameSettings();
            var loader = new JsonWebDataLoader<WordsData>(URL, gameSettings.SaveDataFileName);
            loader.LoadData().ContinueWith(x => wordsData = loader.Data);
            Fillword = new FillwordTableRandomBuilder(wordsData, gameSettings.Difficulty).Build();
        }
    }
}
