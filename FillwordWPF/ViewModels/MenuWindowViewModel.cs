using FillwordWPF.Commands;
using FillwordWPF.Services;
using System.Windows;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class MenuWindowViewModel : ViewModel
    {
        public ICommand ShowMetaInfoCommand { get; }
        public ICommand StartNewGameCommand { get; }
        public ICommand ChangeDifficultyCommand { get; }
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
            MessageBox.Show(new JsonObjectSerializer().Deserialize<string>("gameinfo.json"));
        }
    }
}
