using FillwordWPF.Commands;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FillwordWPF.ViewModels
{
    internal class NewGameViewModel : ViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IWritableOptions<GameSettings> gameOptions;
        private readonly DownloadDataService downloadDataService;
        private readonly FillwordViewModel fillwordViewModel;
        private readonly GameProcessService gameProcessService;
        private readonly Dictionary<string, object> tempChanges;
        private bool isSavedLoaded;

        private ObjectSerializer objectSerializer;

        public int Size
        {
            get => tempChanges.ContainsKey(nameof(Size))
                ? (int)tempChanges[nameof(Size)]
                : gameOptions.Value.Size;
            set
            {
                tempChanges[nameof(Size)] = value;
                ReloadFillword(value);
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

        private ObservableCollection<Save> savedFillwords;
        public ObservableCollection<Save> SavedFillwords
        {
            get => savedFillwords;
            set => Set(ref savedFillwords, value);
        }

        private Save selectedSave;
        public Save SelectedSave
        {
            get => selectedSave;
            set
            {
                if (value != null && Set(ref selectedSave, value))
                {
                    isSavedLoaded = true;
                    ReloadFillword(Fillword.Load(value.FilePath, objectSerializer));
                }
            }
        }

        public ICommand NavigateToMenuCommand { get; }
        public ICommand ReloadFillwordCommand { get; }
        public ICommand NavigateToNewGameCommand { get; }
        public ICommand ResetChangesCommand { get; }
        public ICommand ResetProgressCommand { get; }
        public ICommand DeleteSaveCommand { get; }

        public NewGameViewModel(
            INavigationService navigationService,
            IWritableOptions<GameSettings> gameOptions,
            DownloadDataService downloadDataService,
            FillwordViewModel fillwordViewModel,
            GameProcessService gameProcessService,
            ObjectSerializer objectSerializer)
        {
            this.objectSerializer = objectSerializer;
            this.navigationService = navigationService;
            this.gameOptions = gameOptions;
            this.downloadDataService = downloadDataService;
            this.fillwordViewModel = fillwordViewModel;
            this.gameProcessService = gameProcessService;

            tempChanges = new Dictionary<string, object>();

            GetSaves();
            gameOptions.OnUpdateEvent += GetSaves;
            gameProcessService.GameStarted += GetSaves;
            gameProcessService.GameStoped += GetSaves;
            gameProcessService.GameStoped += OnWin;

            NavigateToMenuCommand = new RelayCommand(x =>
            {
                navigationService.NavigateTo<MainMenuViewModel>();
            });

            NavigateToNewGameCommand = new RelayCommand(x =>
            {
                if (!isSavedLoaded && isInLoading)
                {
                    //...
                }
                else
                { 
                    SaveChanges();
                    gameProcessService.StartGame(isSavedLoaded);
                    navigationService.NavigateTo<GameViewModel>();
                }
            });

            ResetChangesCommand = new RelayCommand(x => ResetChanges());

            ReloadFillwordCommand = new RelayCommand(x => ReloadFillword(Size));

            DeleteSaveCommand = new RelayCommand(DeleteSeletedSave);

            ResetProgressCommand = new RelayCommand(OnResetProgress);

            downloadDataService.ProgressChanged += DownloadDataService_ProgressChanged;

            DataDownloadAsync();
        }

        private void OnResetProgress(object obj)
        {
            var size = fillwordViewModel.Fillword.GameProcessService.ColorsMap.Length;
            fillwordViewModel.Fillword.GameProcessService.ColorsMap = new string[size, size];
        }

        private void DeleteSeletedSave(object parameter)
        {
            if (parameter is null)
                return;

            var selected = (Save)parameter;

            File.Delete(selected.FilePath);

            if (SavedFillwords.Count != 0 && !SavedFillwords.Remove(selected))
                throw new InvalidOperationException();

            if (SavedFillwords.Count == 0)
            {
                ReloadFillword(Size);
            }
            else
            {
                SelectedSave = SavedFillwords.First();
            }
        }

        private void OnWin()
        {
            gameProcessService.ColorsMap = new string[Size, Size];

            MessageBox.Show("You winner!");
            navigationService.NavigateTo<NewGameViewModel>();
            ReloadFillword(Size);
        }

        private void ReloadFillword(Fillword fillword)
        {
            gameProcessService.StopGame(true);
            fillwordViewModel.Fillword = fillword;
            gameProcessService.ColorsMap = fillword.GameProcessService.ColorsMap;
        }

        private void ReloadFillword(int size)
        {
            gameProcessService.ColorsMap = new string[size, size];
            var newFillword = Fillword.CreateRandom(size, gameProcessService, new JsonObjectSerializer());

            isSavedLoaded = false;
            ReloadFillword(newFillword);
        }

        private async Task DataDownloadAsync()
        {
            if (downloadDataService.IsDisposed)
                return;

            using (downloadDataService)
            {
                await downloadDataService.StartDownload();
            }
        }

        private void GetSaves()
        {
            var saves = Save.GetSaves(objectSerializer).Reverse();
            SavedFillwords = new ObservableCollection<Save>(saves);
        }

        private async void DownloadDataService_ProgressChanged(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        {
            DownloadProgressLevel = progressPercentage.Value;

            if (totalFileSize == totalBytesDownloaded || isSavedLoaded)
            {
                await Task.Delay(300);
                IsInLoading = false;
                downloadDataService.ProgressChanged -= DownloadDataService_ProgressChanged;

                if (!isSavedLoaded)
                    ReloadFillword(Size);
            }
        }

        private void SaveChanges()
        {
            var type = gameOptions.Value.GetType();
            foreach (var item in tempChanges)
            {
                var property = type.GetProperty(item.Key);
                gameOptions.Update(x => property.SetValue(x, item.Value));
            }
        }

        private void ResetChanges()
        {
            tempChanges.Clear();
            foreach (var property in GetType().GetProperties())
                OnPropertyChanged(property.Name);
        }
    }
}
