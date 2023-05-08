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
        private Fillword fillwordCurrent;
        private bool isLoaded;


        private int size;
        public int Size
        {
            get => size;
            set
            {
                if (Set(ref size, value) && isLoaded)
                    ReloadFillword();
            }
        }

        private ObservableCollection<FillwordItem> fillwordItemsLinear;
        public ObservableCollection<FillwordItem> FillwordItemsLinear
        {
            get => fillwordItemsLinear;
            set => Set(ref fillwordItemsLinear, value);
        }

        public FillwordViewModel(
            IWritableOptions<GameSettings> options,
            INavigationService navigationService,
            DownloadDataService downloadDataService,
            GameProcessService gameProcessService)
        {
            FillwordItemsLinear = new ObservableCollection<FillwordItem>();

            this.options = options;
            this.navigationService = navigationService;
            this.gameProcessService = gameProcessService;
            this.size = options.Value.Size;

            gameProcessService.GameStartsEvent += OnGameProgressChanged;
            gameProcessService.GameProgressChangedEvent += OnGameProgressChanged;
            gameProcessService.GameEndsEvent += OnWin;

            if (!App.IsDesignMode)
            {
                StartService(downloadDataService);
            }
        }

        private void OnGameProgressChanged()
        {
            fillwordCurrent?.SaveAsync();
        }

        private void OnWin()
        {
            MessageBox.Show("You winner!");

            navigationService.NavigateTo<NewGameViewModel>();

            ReloadFillword();
        }

        public async void StartService(DownloadDataService downloadDataService)
        {
            downloadDataService.SuccessfullyDownloaded += async (a, b, c) =>
            {
                isLoaded = true;
                await Task.Delay(100);
                ReloadFillword();
            };
            await DataDownloadAsync(downloadDataService);
        }

        public void ReloadFillword()
        {
            var data = new JsonObjectSerializer().Deserialize<WordsData>(App.LoadedDataFileName);
            var table = new FillwordTableRandomBuilder(data, Size).Build();

            FillwordItemsLinear = new ObservableCollection<FillwordItem>(table.AsLinear());

            fillwordCurrent = new Fillword
            {
                FillwordItemsLinear = FillwordItemsLinear,
                GameProcessService = gameProcessService,
                InitTime = DateTime.Now
            };
        }

        public async Task DataDownloadAsync(DownloadDataService downloadDataService)
        {
            if (downloadDataService.IsDisposed)
                return;

            using (downloadDataService)
            {
                await downloadDataService.StartDownload();
            }
        }
    }
}
