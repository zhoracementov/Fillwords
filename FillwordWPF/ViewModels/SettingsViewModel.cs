using FillwordWPF.Commands;
using FillwordWPF.Infrastructure;
using FillwordWPF.Models;
using FillwordWPF.Services.Navigation;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class SettingsViewModel : ViewModel
    {
        private readonly GameSettingsService gameSettings;

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

        public ICommand NavigateToMenuCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand ResetChangesCommand { get; }

        public SettingsViewModel(INavigationService navigateService)
        {
            gameSettings = App.GameSettings;
            SaveChangesCommand = new RelayCommand(x => gameSettings.Save());
            NavigateToMenuCommand = new RelayCommand(x => navigateService.NavigateTo<MainMenuViewModel>());
        }
    }
}
