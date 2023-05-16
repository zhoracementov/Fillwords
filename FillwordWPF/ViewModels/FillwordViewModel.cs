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
        private readonly INavigationService navigationService;
        private readonly GameProcessService gameProcessService;
        private readonly ObjectSerializer objectSerializer;

        private Fillword fillword;
        public Fillword Fillword
        {
            get => fillword;
            set => Set(ref fillword, value);
        }

        public FillwordViewModel(
            INavigationService navigationService,
            DownloadDataService downloadDataService,
            GameProcessService gameProcessService,
            ObjectSerializer objectSerializer)
        {
            this.navigationService = navigationService;
            this.gameProcessService = gameProcessService;
            this.objectSerializer = objectSerializer;

            gameProcessService.GameStarted += OnGameProgressChanged;
            gameProcessService.GameProgressChanged += OnGameProgressChanged;
        }

        private void OnGameProgressChanged()
        {
            fillword?.SaveAsync(objectSerializer);
        }
    }
}
