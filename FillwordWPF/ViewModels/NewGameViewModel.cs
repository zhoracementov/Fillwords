using FillwordWPF.Commands;
using FillwordWPF.Game;
using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.Services.Navigation;
using FillwordWPF.Services.Serializers;
using FillwordWPF.Services.WriteableOptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class NewGameViewModel : ViewModel
    {
        private readonly IWritableOptions<GameSettings> gameOptions;
        private readonly IDictionary<string, object> tempChanges;

        private FillwordItem[,] fillwordItems;

        public FillwordItem[,] FillwordItems
        {
            get => fillwordItems;
            set => Set(ref fillwordItems, value);
        }

        public int Size
        {
            get => tempChanges.ContainsKey(nameof(Size))
                ? (int)tempChanges[nameof(Size)]
                : gameOptions.Value.Size;
            set
            {
                tempChanges[nameof(Size)] = value;
                OnPropertyChanged();
            }
        }

        private double downloadProgressLevel;
        public double DownloadProgressLevel
        {
            get => downloadProgressLevel;
            set => Set(ref downloadProgressLevel, value);
        }

        public ICommand NavigateToMenuCommand { get; }
        public ICommand NavigateToNewGameCommand { get; }
        public ICommand ResetChangesCommand { get; }

        public NewGameViewModel(INavigationService navigationService, IWritableOptions<GameSettings> gameOptions)
        {
            this.gameOptions = gameOptions;
            tempChanges = new Dictionary<string, object>();

            NavigateToMenuCommand = new RelayCommand(x => navigationService.NavigateTo<MainMenuViewModel>());
            NavigateToNewGameCommand = new RelayCommand(x =>
            {
                SaveChanges();
                navigationService.NavigateTo<GameViewModel>();
            });

            ResetChangesCommand = new RelayCommand(x => ResetChanges());

            if (!App.IsDesignMode)
            {
                DataDownload().ContinueWith(x => CreateFillword());
            }
        }

        private async Task CreateFillword()
        {
            FillwordItems = new FillwordTableRandomBuilder(
                data ??= await new JsonObjectSerializer()
                .DeserializeAsync<WordsData>(App.LoadedDataFileName), Size)
                .Build();
        }

        private WordsData data;

        private async Task DataDownload()
        {
            using (var manager = new DownloadDataService(App.URL, App.LoadedDataFileName))
            {
                manager.ProgressChanged += Manager_ProgressChanged;
                await manager.StartDownload();
            }
        }

        private async Task<T> DataParse<T>(T data) where T : IDictionary<string, WordInfo>
        {
            var json = new JsonObjectSerializer();
            return await json.DeserializeAsync<T>(App.LoadedDataFileName);
        }

        private void Manager_ProgressChanged(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        {
            DownloadProgressLevel = progressPercentage.Value;
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

        public void ResetChanges()
        {
            tempChanges.Clear();
            foreach (var property in GetType().GetProperties())
                OnPropertyChanged(property.Name);
        }
    }
}
