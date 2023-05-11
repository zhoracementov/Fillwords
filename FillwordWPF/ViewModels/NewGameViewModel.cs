﻿using FillwordWPF.Commands;
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
                    ReloadFillword(value.GetFillword());
                }
            }
        }

        public ICommand NavigateToMenuCommand { get; }
        public ICommand ReloadFillwordCommand { get; }
        public ICommand NavigateToNewGameCommand { get; }
        public ICommand ResetChangesCommand { get; }

        public ICommand DeleteSaveCommand { get; }

        public NewGameViewModel(
            INavigationService navigationService,
            IWritableOptions<GameSettings> gameOptions,
            DownloadDataService downloadDataService,
            FillwordViewModel fillwordViewModel,
            GameProcessService gameProcessService)
        {
            GetSaves();
            gameOptions.OnUpdateEvent += GetSaves;
            gameProcessService.GameStarts += GetSaves;
            gameProcessService.GameEnds += GetSaves;
            gameProcessService.GameEnds += OnWin;

            this.navigationService = navigationService;
            this.gameOptions = gameOptions;
            this.downloadDataService = downloadDataService;
            this.fillwordViewModel = fillwordViewModel;
            this.gameProcessService = gameProcessService;

            tempChanges = new Dictionary<string, object>();

            NavigateToMenuCommand = new RelayCommand(x =>
            {
                navigationService.NavigateTo<MainMenuViewModel>();
            });

            NavigateToNewGameCommand = new RelayCommand(x =>
            {
                SaveChanges();
                gameProcessService.StartGame(isSavedLoaded);
                navigationService.NavigateTo<GameViewModel>();
            });

            ResetChangesCommand = new RelayCommand(x =>
            {
                ResetChanges();
            });

            ReloadFillwordCommand = new RelayCommand(x => ReloadFillword(Size));

            DeleteSaveCommand = new RelayCommand(DeleteSeletedSave);

            downloadDataService.ProgressChanged += DownloadDataService_ProgressChanged;

            DataDownloadAsync();
        }

        private void DeleteSeletedSave(object parameter)
        {
            var selected = (Save)parameter;

            File.Delete(selected.FilePath);

            if (!SavedFillwords.Remove(selected))
                throw new InvalidOperationException();

            if (SavedFillwords.Count == 0)
                ReloadFillword(Size);
        }

        private void OnWin()
        {
            gameProcessService.SolvedMap = new bool[Size, Size];
            gameProcessService.ColorsMap = new string[Size, Size];

            MessageBox.Show("You winner!");
            navigationService.NavigateTo<NewGameViewModel>();
            ReloadFillword(Size);
        }

        public void ReloadFillword(Fillword fillword)
        {
            fillwordViewModel.Fillword = fillword;
            gameProcessService.SolvedMap = fillword.GameProcessService.SolvedMap;
            gameProcessService.ColorsMap = fillword.GameProcessService.ColorsMap;
        }

        public void ReloadFillword(int size)
        {
            var data = new JsonObjectSerializer().Deserialize<WordsData>(App.LoadedDataFileName);
            var table = new FillwordTableRandomBuilder(data, size).Build();
            var linear = new ObservableCollection<FillwordItem>(table.AsLinear());

            var newFillword = new Fillword
            {
                ItemsLinear = linear,
                GameProcessService = gameProcessService,
                InitTime = DateTime.Now,
                Size = size
            };

            newFillword.GameProcessService.SolvedMap = new bool[size, size];
            newFillword.GameProcessService.ColorsMap = new string[size, size];

            isSavedLoaded = false;
            ReloadFillword(newFillword);
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

        private void GetSaves()
        {
            var saves = Save.GetSaves(new JsonObjectSerializer()).Reverse();
            SavedFillwords = new ObservableCollection<Save>(saves);
        }

        private async void DownloadDataService_ProgressChanged(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        {
            DownloadProgressLevel = progressPercentage.Value;

            if (totalFileSize == totalBytesDownloaded)
            {
                await Task.Delay(300);
                IsInLoading = false;
                downloadDataService.ProgressChanged -= DownloadDataService_ProgressChanged;
                ReloadFillword(Size);
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
