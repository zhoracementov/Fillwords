using FillwordWPF.ViewModels;

namespace FillwordWPF.Services
{
    internal interface INavigationService
    {
        public ViewModel CurrentViewModel { get; }
        void NavigateTo<T>() where T : ViewModel;
    }
}
