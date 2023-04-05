using FillwordWPF.Commands;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class NewGamePageViewModel : ViewModel
    {
        public ICommand BackToMenuCommand { get; }
    }
}
