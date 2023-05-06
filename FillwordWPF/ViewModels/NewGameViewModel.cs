using FillwordWPF.Commands;
using FillwordWPF.Game;
using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.Services.Navigation;
using FillwordWPF.Services.Serializers;
using FillwordWPF.Services.WritableOptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class NewGameViewModel : ViewModel
    {
        private readonly IWritableOptions<GameSettings> gameOptions;
        private readonly DownloadDataService downloadDataService;
        private readonly FillwordViewModel fillwordViewModel;
        private readonly Dictionary<string, object> tempChanges;

        public int Size
        {
            get => tempChanges.ContainsKey(nameof(Size))
                ? (int)tempChanges[nameof(Size)]
                : gameOptions.Value.Size;
            set
            {
                tempChanges[nameof(Size)] = value;
                fillwordViewModel.Size = value;
                OnPropertyChanged();
            }
        }

        private double downloadProgressLevel;
        public double DownloadProgressLevel
        {
            get => downloadProgressLevel;
            set => Set(ref downloadProgressLevel, value);
        }

        private bool isInLoading = !App.IsDesignMode;
        public bool IsInLoading
        {
            get => isInLoading;
            set
            {
                if (Set(ref isInLoading, value))
                    OnPropertyChanged(nameof(IsShowSlider));
            }
        }

        public bool IsShowSlider => !IsInLoading;

        public ICommand NavigateToMenuCommand { get; }
        public ICommand ReloadFillwordCommand { get; }
        public ICommand NavigateToNewGameCommand { get; }
        public ICommand ResetChangesCommand { get; }

        public NewGameViewModel(
            INavigationService navigationService,
            IWritableOptions<GameSettings> gameOptions,
            DownloadDataService downloadDataService,
            FillwordViewModel fillwordViewModel,
            GameProcessService gameProcessService)
        {
            this.gameOptions = gameOptions;
            this.downloadDataService = downloadDataService;
            this.fillwordViewModel = fillwordViewModel;

            tempChanges = new Dictionary<string, object>();

            NavigateToMenuCommand = new RelayCommand(x =>
            {
                navigationService.NavigateTo<MainMenuViewModel>();
            });

            NavigateToNewGameCommand = new RelayCommand(x =>
            {
                SaveChanges();
                gameProcessService.IsGameActive = true;
                navigationService.NavigateTo<GameViewModel>();
            });

            ResetChangesCommand = new RelayCommand(x =>
            {
                ResetChanges();
                fillwordViewModel.Size = Size;
            });

            ReloadFillwordCommand = new RelayCommand(x => fillwordViewModel.CreateFillwordAsync());

            downloadDataService.ProgressChanged += DownloadDataService_ProgressChanged;
        }

        private async void DownloadDataService_ProgressChanged(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        {
            DownloadProgressLevel = progressPercentage.Value;

            if (totalFileSize == totalBytesDownloaded)
            {
                await Task.Delay(300);
                IsInLoading = false;
                downloadDataService.ProgressChanged -= DownloadDataService_ProgressChanged;
            }
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
