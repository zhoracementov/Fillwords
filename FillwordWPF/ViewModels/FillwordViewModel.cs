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
                    CreateFillwordAsync();
            }
        }

        private ObservableCollection<FillwordItem> fillwordItemsLinear;
        public ObservableCollection<FillwordItem> FillwordItemsLinear
        {
            get => fillwordItemsLinear;
            set => Set(ref fillwordItemsLinear, value);
        }

        public ICommand SelectNextItemCommand { get; }
        public ICommand StartSelectCommand { get; }
        public ICommand EndSelectCommand { get; }

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

            SelectNextItemCommand = new RelayCommand(OnSelectNextItem);
            StartSelectCommand = new RelayCommand(OnStartSelectCommand);
            EndSelectCommand = new RelayCommand(OnEndSelectCommand);

            gameProcessService.GameStartsEvent += OnGameProgressChanged;
            gameProcessService.GameProgressChangedEvent += OnGameProgressChanged;
            gameProcessService.GameEndsEvent += OnGameProgressChanged;

            if (!App.IsDesignMode)
            {
                StartService(downloadDataService);
            }
        }

        private async void OnGameProgressChanged()
        {
            await fillwordCurrent?.SaveAsync();
        }

        public void OnSelectNextItem(object parameter)
        {
            var item = (FillwordItem)parameter;
            var index = item.Point.GetLinearArrayIndex(Size, Size);
            var itemInArr = fillwordItemsLinear[index];
            var eq = item == itemInArr;

            if (gameProcessService.OnSelectNextItem(item))
            {
                //...
            }
        }

        public void OnEndSelectCommand(object parameter)
        {
            var ans = gameProcessService.OnEndSelecting();

            if (ans.SolvedAll)
            {
                MessageBox.Show("You winner!");

                navigationService.NavigateTo<NewGameViewModel>();
                gameProcessService.IsGameActive = false;

                CreateFillwordAsync();
            }
        }

        public void OnStartSelectCommand(object parameter)
        {
            gameProcessService.OnStartSelecting((FillwordItem)parameter);
        }

        public async void StartService(DownloadDataService downloadDataService)
        {
            downloadDataService.SuccessfullyDownloaded += async (a, b, c) =>
            {
                isLoaded = true;
                await Task.Delay(100);
                CreateFillwordAsync();
            };
            await DataDownloadAsync(downloadDataService);
        }

        public async void CreateFillwordAsync()
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
