using FillwordWPF.Commands;
using FillwordWPF.Services;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class MenuWindowViewModel : ViewModel
    {
        public const string MetaInfoFileName = "meta.json";

        public ICommand ShowMetaInfoCommand { get; }
        public ICommand StartNewGameCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand CloseAppCommand { get; }

        public MenuWindowViewModel()
        {
            ShowMetaInfoCommand = new RelayCommand(x => ShowMetaInfo());
            CloseAppCommand = new RelayCommand(x => CloseApplication());
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
