using Microsoft.Extensions.DependencyInjection;

namespace FillwordWPF.ViewModels
{
    internal class ViewModelsLocator
    {
        public MainWindowViewModel MainWindowViewModel => GetViewModel<MainWindowViewModel>();
        public MenuWindowViewModel MenuWindowViewModel => GetViewModel<MenuWindowViewModel>();
        public NewGamePageViewModel NewGamePageViewModel => GetViewModel<NewGamePageViewModel>();
        public GameSettingsViewModel GameSettingsViewModel => GetViewModel<GameSettingsViewModel>();

        public TViewModel GetViewModel<TViewModel>() where TViewModel : ViewModel
        {
            return App.Host.Services.GetRequiredService<TViewModel>();
        }
    }
}
