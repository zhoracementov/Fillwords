using FillwordWPF.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FillwordWPF.Services.Navigation
{
    internal class NavigationService : INavigationService, INotifyPropertyChanged
    {
        private readonly Func<Type, ViewModel> viewModelFactory;
        private ViewModel currentViewModel;
        public ViewModel CurrentViewModel
        {
            get => currentViewModel;
            private set
            {
                currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public NavigationService(Func<Type, ViewModel> viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            CurrentViewModel = viewModelFactory.Invoke(typeof(TViewModel));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
