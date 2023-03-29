using FillwordWPF.Infrastructure;
using FillwordWPF.Models;

namespace FillwordWPF.ViewModels
{
    internal class GameSettingsViewModel : ViewModel
    {
        private Difficulty difficulty;
        public Difficulty Difficulty
        {
            get => difficulty;
            set => Set(ref difficulty, value);
        }

        private int minWordLength;
        public int MinWordLength
        {
            get => minWordLength;
            set => Set(ref minWordLength, value);
        }

        private string saveDataFileName;
        public string SaveDataFileName
        {
            get => saveDataFileName;
            set => Set(ref saveDataFileName, value);
        }

        public GameSettingsViewModel()
        {
            var gameSettings = new GameSettings();
            Difficulty = gameSettings.Difficulty;
            MinWordLength = gameSettings.MinWordLength;
            SaveDataFileName = gameSettings.SaveDataFileName;
        }
    }
}
