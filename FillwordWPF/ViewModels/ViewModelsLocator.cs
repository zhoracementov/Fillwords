﻿using Microsoft.Extensions.DependencyInjection;

namespace FillwordWPF.ViewModels
{
    internal class ViewModelsLocator
    {
        public MainWindowViewModel MainWindowViewModel => GetViewModel<MainWindowViewModel>();
        public MainMenuViewModel MenuWindowViewModel => GetViewModel<MainMenuViewModel>();
        public NewGameViewModel NewGamePageViewModel => GetViewModel<NewGameViewModel>();
        public SettingsViewModel GameSettingsViewModel => GetViewModel<SettingsViewModel>();
        public GameViewModel GameViewModel => GetViewModel<GameViewModel>();
        public FillwordViewModel FillwordViewModel => GetViewModel<FillwordViewModel>();

        private TViewModel GetViewModel<TViewModel>() where TViewModel : ViewModel
        {
            return App.Host.Services.GetRequiredService<TViewModel>();
        }
    }
}
