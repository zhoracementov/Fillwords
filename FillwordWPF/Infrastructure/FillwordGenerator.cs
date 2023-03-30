using FillwordWPF.Models;
using FillwordWPF.Services;
using System;
using System.Threading.Tasks;

namespace FillwordWPF.Infrastructure
{
    internal class FillwordGenerator
    {
        private const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";
        public Task<Fillword> LoadingTask { get; }

        public FillwordGenerator()
        {
            WordsData wordsData = null;
            var gameSettings = new GameSettings();
            var loader = new JsonWebDataLoader<WordsData>(URL, gameSettings.SaveDataFileName);

            LoadingTask = loader
                .LoadData()
                .ContinueWith(x => new FillwordTableRandomBuilder(wordsData, gameSettings.Difficulty)
                .Build());
        }
    }
}
