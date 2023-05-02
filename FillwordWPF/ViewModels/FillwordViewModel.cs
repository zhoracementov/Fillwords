using FillwordWPF.Commands;
using FillwordWPF.Extenstions;
using FillwordWPF.Game;
using FillwordWPF.Extenstions;
using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.Services.Serializers;
using FillwordWPF.Services.WriteableOptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class FillwordViewModel : ViewModel
    {
        private readonly IWritableOptions<GameSettings> options;
        private readonly GameProcessService gameProcessService;
        private WordsData data;

        private int size;
        public int Size
        {
            get => size;
            set
            {
                if (Set(ref size, value) && (data != null))
                {
                    CreateFillwordAsync();
                }
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

        public FillwordViewModel(IWritableOptions<GameSettings> options, DownloadDataService downloadDataService, GameProcessService gameProcessService)
        {
            FillwordItemsLinear = new ObservableCollection<FillwordItem>();

            this.options = options;
            this.gameProcessService = gameProcessService;
            this.Size = options.Value.Size;

            SelectNextItemCommand = new RelayCommand(OnSelectNextItem);
            StartSelectCommand = new RelayCommand(OnStartSelectCommand);
            EndSelectCommand = new RelayCommand(OnEndSelectCommand);

            if (!App.IsDesignMode)
            {
                StartService(downloadDataService);
            }
        }

        public void OnSelectNextItem(object parameter)
        {
            gameProcessService.OnSelectNextItem((FillwordItem)parameter);
        }

        public void OnEndSelectCommand(object parameter)
        {
            gameProcessService.OnEndSelecting();
        }

        public void OnStartSelectCommand(object parameter)
        {
            gameProcessService.OnStartSelecting((FillwordItem)parameter);
        }

        public async void StartService(DownloadDataService downloadDataService)
        {
            downloadDataService.SuccessfullyDownloaded += (a, b, c) => CreateFillwordAsync();
            await DataDownloadAsync(downloadDataService);
        }

        public async void CreateFillwordAsync()
        {
            var fillwordItems = await Task.Run(async () =>
                new FillwordTableRandomBuilder(
                data ??= await new JsonObjectSerializer()
                .DeserializeAsync<WordsData>(App.LoadedDataFileName), Size)
                .Build()
                .AsLinear());

            FillwordItemsLinear = new ObservableCollection<FillwordItem>(fillwordItems);
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
