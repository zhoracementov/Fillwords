using FillwordWPF.ViewModels;
using System;

namespace FillwordWPF.Services
{
    internal class NavigationService : ViewModel, INavigationService
    {
        private readonly Func<Type, ViewModel> viewModelFactory;
        private ViewModel currentViewModel;
        public ViewModel CurrentViewModel
        {
            get => currentViewModel;
            private set
            {
                OnPropertyChanged();
                currentViewModel = value;
            }
        }

        public NavigationService(Func<Type, ViewModel> viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            CurrentViewModel = viewModelFactory.Invoke(typeof(TViewModel));

            throw new NotImplementedException();
        }
    }
}
