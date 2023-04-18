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

        public FillwordViewModel(IWritableOptions<GameSettings> options, DownloadDataService downloadDataService)
        {
            FillwordItemsLinear = new ObservableCollection<FillwordItem>();

            if (!App.IsDesignMode)
            {
                StartService(downloadDataService);
                this.options = options;
                this.Size = options.Value.Size;
            }
            else
            {
                this.Size = 4;
                var words = new string[] { "test1, test2, test3, test4, test5" };
                var items = (WordsData)words.ToDictionary(k => k, v => (WordInfo)null);

                var fillwordItems = new FillwordTableRandomBuilder(
                    data ??= new JsonObjectSerializer()
                    .Deserialize<WordsData>(App.LoadedDataFileName), Size)
                    .Build()
    .               AsLinear();

                FillwordItemsLinear = new ObservableCollection<FillwordItem>(fillwordItems);
            }
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
