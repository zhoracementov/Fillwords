using FillwordWPF.ViewModels;

namespace FillwordWPF.Services.Navigation
{
    internal interface INavigationService
    {
        public ViewModel CurrentViewModel { get; }
        void NavigateTo<T>() where T : ViewModel;
    }
}
