using FillwordWPF.Commands;
using FillwordWPF.Services.Navigation;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class SettingsViewModel : ViewModel
    {
        public ICommand NavigateToMenuCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand ResetChangesCommand { get; }

        public SettingsViewModel(INavigationService navigateService)
        {
            NavigateToMenuCommand = new RelayCommand(x => navigateService.NavigateTo<MainMenuViewModel>());
        }
    }
}
