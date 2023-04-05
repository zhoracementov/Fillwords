using FillwordWPF.Commands;
using FillwordWPF.Infrastructure;
using FillwordWPF.Models;
using System.Windows.Input;

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
                if (value >= Difficulty.Easy && value <= Difficulty.Hard)
                {
                    gameSettings.Difficulty = value;
                    OnPropertyChanged(nameof(Difficulty));
                }
            }
        }

        public int MinWordLength
        {
            get => gameSettings.MinWordLength;
            set
            {
                if (value >= FillwordTableBuilder.MinWordLength)
                {
                    gameSettings.MinWordLength = value;
                    OnPropertyChanged(nameof(MinWordLength));
                }
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

        public ICommand BackToMenuCommand { get; }
        public ICommand SaveChanges { get; }
        public ICommand ResetChanges { get; }

        public GameSettingsViewModel()
        {
            gameSettings = App.GameSettings;
            SaveChanges = new RelayCommand(x => gameSettings.Save());
        }
    }
}
