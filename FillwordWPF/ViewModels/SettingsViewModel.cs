using FillwordWPF.Commands;
using FillwordWPF.Services;
using FillwordWPF.Services.Navigation;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class SettingsViewModel : ViewModel
    {
        public ICommand NavigateToMenuCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand ResetChangesCommand { get; }

        private string currentColor;
        public string CurrentColor
        {
            get => currentColor;
            set => Set(ref currentColor, value);
        }

        public ICommand ChangeColorCommand { get; }

        public SettingsViewModel(INavigationService navigateService, BrushesNamesLoopQueue loopQueue)
        {
            CurrentColor = loopQueue.StartString;
            ChangeColorCommand = new RelayCommand(x => CurrentColor = loopQueue.NextString);
            NavigateToMenuCommand = new RelayCommand(x => navigateService.NavigateTo<MainMenuViewModel>());
        }
    }
}
