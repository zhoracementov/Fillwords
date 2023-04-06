using FillwordWPF.Commands;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class NewGameViewModel : ViewModel
    {
        public ICommand BackToMenuCommand { get; }
    }
}
