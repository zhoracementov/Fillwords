using FillwordWPF.Infrastructure;
using FillwordWPF.Models;

namespace FillwordWPF.ViewModels
{
    internal class GameSettingsViewModel : ViewModel
    {
        private readonly GameSettings gameSettings;

        public Difficulty Difficulty
        {
            get => gameSettings.Difficulty;
            set
            {
                gameSettings.Difficulty = value;
                OnPropertyChanged(nameof(Difficulty));
            }
        }

        public int MinWordLength
        {
            get => gameSettings.MinWordLength;
            set
            {
                gameSettings.MinWordLength = value;
                OnPropertyChanged(nameof(MinWordLength));
            }
        }

        public string SaveDataFileName
        {
            get => gameSettings.SaveDataFileName;
            set
            {
                gameSettings.SaveDataFileName = value;
                OnPropertyChanged(nameof(SaveDataFileName));
            }
        }

        public GameSettingsViewModel()
        {
            gameSettings = App.GameSettings;
        }
    }
}
