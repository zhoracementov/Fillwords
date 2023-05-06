using FillwordWPF.Commands;
using FillwordWPF.Services;
using FillwordWPF.Services.Navigation;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public string Title => App.Version;

        private INavigationService navigationService;
        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

        public ICommand NavigateToMenuCommand { get; }

        public MainWindowViewModel(INavigationService navigationService, GameProcessService gameProcessService)
        {
            NavigationService = navigationService;
            NavigationService.NavigateTo<MainMenuViewModel>();
            NavigateToMenuCommand = new RelayCommand(x =>
            {
                NavigationService.NavigateTo<MainMenuViewModel>();
                gameProcessService.IsGameActive = false;
            });
        }
    }
}
