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

namespace FillwordWPF.ViewModels
{
    internal class FillwordViewModel : ViewModel
    {
        private readonly IWritableOptions<GameSettings> options;
        private readonly DownloadDataService downloadDataService;
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

        public FillwordViewModel(IWritableOptions<GameSettings> options, DownloadDataService downloadDataService)
        {
            this.options = options;
            this.downloadDataService = downloadDataService;
            this.Size = options.Value.Size;

            if (!App.IsDesignMode)
            {
                DataDownloadAsync().ContinueWith(x => CreateFillwordAsync());
            }
        }

        public async Task CreateFillwordAsync()
        {
            var fillwordItems = new FillwordTableRandomBuilder(
                data ??= await new JsonObjectSerializer()
                .DeserializeAsync<WordsData>(App.LoadedDataFileName), Size)
                .Build()
                .AsLinear();

            FillwordItemsLinear = new ObservableCollection<FillwordItem>(fillwordItems);
        }

        public async Task DataDownloadAsync()
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
