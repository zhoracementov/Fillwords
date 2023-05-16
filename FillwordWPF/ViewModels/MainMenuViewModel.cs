using FillwordWPF.Commands;
using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.Services.Navigation;
using FillwordWPF.Services.Serializers;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class MainMenuViewModel : ViewModel
    {
        public ICommand ShowMetaInfoCommand { get; }
        public ICommand StartNewGameCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand CloseAppCommand { get; }

        public MainMenuViewModel(INavigationService navigationService)
        {
            ShowMetaInfoCommand = new RelayCommand(x => ShowMetaInfo());
            CloseAppCommand = new RelayCommand(x => CloseApplication());
            OpenSettingsCommand = new RelayCommand(x => navigationService.NavigateTo<SettingsViewModel>());
            StartNewGameCommand = new RelayCommand(x => navigationService.NavigateTo<NewGameViewModel>());
        }

        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        private void ShowMetaInfo()
        {
            var fileName = Path.Combine(App.DataDirectory, App.MetaInfoFileName);
            if (File.Exists(fileName))
                MessageBox.Show(new JsonObjectSerializer().Deserialize<string>(fileName));
        }
    }
}
