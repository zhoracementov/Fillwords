using FillwordWPF.Commands;
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
        public const string MetaInfoFileName = "meta.json";

        public ICommand ShowMetaInfoCommand { get; }
        public ICommand StartNewGameCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand CloseAppCommand { get; }


        private INavigationService navigationService;
        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

        public MainMenuViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            ShowMetaInfoCommand = new RelayCommand(x => ShowMetaInfo());
            CloseAppCommand = new RelayCommand(x => CloseApplication());
            OpenSettingsCommand = new RelayCommand(x => NavigationService.NavigateTo<SettingsViewModel>());
        }

        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        private void ShowMetaInfo()
        {
            var fileName = Path.Combine(App.CurrentDirectory, MetaInfoFileName);
            if (File.Exists(fileName))
                MessageBox.Show(new JsonObjectSerializer().Deserialize<string>(fileName));
        }
    }
}
