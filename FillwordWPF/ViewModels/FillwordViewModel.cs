using FillwordWPF.Commands;
using FillwordWPF.Game;
using FillwordWPF.Game.Extenstions;
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
        private WordsData data;

        private int size;
        public int Size
        {
            get => size;
            set
            {
                Set(ref size, value);
                CreateFillwordAsync();
            }
        }

        private ObservableCollection<FillwordItem> fillwordItemsLinear;
        public ObservableCollection<FillwordItem> FillwordItemsLinear
        {
            get => fillwordItemsLinear;
            set => Set(ref fillwordItemsLinear, value);
        }


        public ICommand ReloadFillwordCommand { get; }
        
        public FillwordViewModel(IWritableOptions<GameSettings> options, DownloadDataService downloadDataService)
        {
            this.options = options;
            this.Size = options.Value.Size;

            if (!App.IsDesignMode)
            {
                StartService(downloadDataService);
            }
            ReloadFillwordCommand = new RelayCommand(x => CreateFillwordAsync());
        }

        public async void StartService(DownloadDataService downloadDataService)
        {
            await DataDownloadAsync(downloadDataService);
            downloadDataService.SuccessfullyDownloaded += (a, b, c) => CreateFillwordAsync();
        }

        public async void CreateFillwordAsync()
        {
            var fillwordItems = await Task.Run(async () =>
                new FillwordTableRandomBuilder(
                data ??= await new JsonObjectSerializer()
                .DeserializeAsync<WordsData>(App.LoadedDataFileName), Size)
                .Build());

            FillwordItemsLinear = new ObservableCollection<FillwordItem>(fillwordItems.AsLinear());
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
