using FillwordWPF.Infrastructure;
using FillwordWPF.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FillwordWPF.Models
{
    internal static class CreatedGamesPool
    {
        public static Dictionary<DateTime, NewGame> CreatedGames { get; } = new Dictionary<DateTime, NewGame>();
    }

    internal class NewGame
    {
        public DownloadDataService DownloadManager { get; }
        public GameSettingsService GameSettings { get; }
        public ObjectSerializer ObjectSerializer { get; }

        public FillwordItem[,] Fillword { get; private set; }
        public DateTime? CreateTime { get; private set; }
        public bool IsCanBeStarted { get; private set; }

        public event EventHandler StartGame;

        public NewGame(ObjectSerializer serializer)
        {
            ObjectSerializer = serializer;

            GameSettings = App.GameSettings;
            DownloadManager = App.DownloadManager;

            DownloadManager.SuccessfullyDownloaded += DownloadManager_SuccessfullyDownloaded;
        }

        private async void DownloadManager_SuccessfullyDownloaded(object sender, EventArgs args)
        {
            AddToPool(DateTime.Now);
            await LoadData();

            StartGame(this, new EventArgs());
        }

        private async Task LoadData()
        {
            var wordsData = await ObjectSerializer.DeserializeAsync<WordsData>(DownloadManager.OutputFilePath);
            var builder = new FillwordTableRandomBuilder(wordsData, GameSettings);
            Fillword = builder.Build();

            IsCanBeStarted = true;
        }

        private void AddToPool(DateTime time)
        {
            CreateTime = time;
            CreatedGamesPool.CreatedGames[time] = this;
        }
    }
}
