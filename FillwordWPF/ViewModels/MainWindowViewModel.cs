using FillwordWPF.Services.Navigation;

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

        public MainWindowViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigationService.NavigateTo<MainMenuViewModel>();
        }
    }
}
