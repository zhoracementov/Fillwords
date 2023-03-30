using FillwordWPF.Infrastructure;
using FillwordWPF.Models;

namespace FillwordWPF.ViewModels
{
    internal class GameViewModel : ViewModel
    {
        public Game Game { get; set; }

        public GameViewModel()
        {
            Game = new Game();
        }
    }
}
