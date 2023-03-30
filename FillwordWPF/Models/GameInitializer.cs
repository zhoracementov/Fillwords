using FillwordWPF.Services;
using System.Net.Http;

namespace FillwordWPF.Models
{
    internal class GameInitializer
    {
        public const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        public DownloadManager DownloadManager { get; }
        public GameSettings GameSettings { get; }
        public GameInitializer()
        {
            GameSettings = new GameSettings();
            DownloadManager = new DownloadManager(URL, GameSettings.SaveDataFileName);
        }
    }
}
