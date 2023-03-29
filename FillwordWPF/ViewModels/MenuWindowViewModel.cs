using FillwordWPF.Commands;
using System;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class MenuWindowViewModel : ViewModel
    {
        public ICommand CloseAppCommand { get; }
        public ICommand StartNewGameCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand ChangeDifficultyCommand { get; }
        public ICommand ShowMetaInfoCommand { get; }

        public MenuWindowViewModel()
        {
            //if (!App.IsDesignMode)
            //{
            //    throw new NotImplementedException();
            //}

            CloseAppCommand = new RelayCommand(x => CloseApplication());
        }

        private void CloseApplication()
        {
            App.Instance.Shutdown();
        }
    }
}
