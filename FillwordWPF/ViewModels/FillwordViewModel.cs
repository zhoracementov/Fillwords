using FillwordWPF.Commands;
using FillwordWPF.Extenstions;
using FillwordWPF.Extenstions;
using FillwordWPF.Game;
using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.Services.Navigation;
using FillwordWPF.Services.Serializers;
using FillwordWPF.Services.WritableOptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class FillwordViewModel : ViewModel
    {
        private readonly IWritableOptions<GameSettings> options;
        private readonly INavigationService navigationService;
        private readonly GameProcessService gameProcessService;

        private Fillword fillword;
        public Fillword Fillword
        {
            get => fillword;
            set
            {
                if (Set(ref fillword, value))
                    OnPropertyChanged(nameof(Size));
            }
        }

        public FillwordViewModel(
            IWritableOptions<GameSettings> options,
            INavigationService navigationService,
            DownloadDataService downloadDataService,
            GameProcessService gameProcessService)
        {
            this.options = options;
            this.navigationService = navigationService;
            this.gameProcessService = gameProcessService;

            gameProcessService.GameStartsEvent += OnGameProgressChanged;
            gameProcessService.GameProgressChangedEvent += OnGameProgressChanged;
            gameProcessService.GameEndsEvent += OnGameProgressChanged;
        }

        private void OnGameProgressChanged()
        {
            fillword?.SaveAsync();
        }
    }
}
