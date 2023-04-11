using FillwordWPF.Commands;
using FillwordWPF.Services;
using FillwordWPF.Services.Navigation;
using FillwordWPF.Services.WriteableOptions;
using System.Collections.Generic;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class NewGameViewModel : ViewModel
    {
        private readonly IWritableOptions<GameSettings> gameOptions;

        private readonly IDictionary<string, object> tempChanges;

        public int Size
        {
            get => tempChanges.ContainsKey(nameof(Size))
                ? (int)tempChanges[nameof(Size)]
                : gameOptions.Value.Size;
            set
            {
                //gameOptions.Update(x => x.Size = value);
                tempChanges[nameof(Size)] = value;

                OnPropertyChanged();
            }
        }

        public ICommand NavigateToMenuCommand { get; }
        public ICommand NavigateToNewGameCommand { get; }

        public NewGameViewModel(INavigationService navigationService, IWritableOptions<GameSettings> gameOptions)
        {
            this.gameOptions = gameOptions;
            tempChanges = new Dictionary<string, object>();

            NavigateToMenuCommand = new RelayCommand(x =>
            navigationService.NavigateTo<MainMenuViewModel>());

            NavigateToNewGameCommand = new RelayCommand(x =>
            Start(navigationService));
        }

        public void Start(INavigationService navigationService)
        {
            SaveChanges();
            navigationService.NavigateTo<GameViewModel>();
        }

        public void SaveChanges()
        {
            var type = gameOptions.Value.GetType();
            foreach (var item in tempChanges)
            {
                var property = type.GetProperty(item.Key);
                gameOptions.Update(x => property.SetValue(x, item.Value));
            }
        }
    }
}
