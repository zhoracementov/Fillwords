using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.ViewModels;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FillwordWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        public static bool IsDesignMode { get; set; } = true;
        public static GameSettings GameSettings { get; set; } = new GameSettings(SettingsFileName);
        public static DownloadManager DownloadManager { get; set; } = new DownloadManager(URL, SaveDataFileName);

        protected override async void OnStartup(StartupEventArgs e)
        {
            await DownloadManager.StartDownload();

            IsDesignMode = false;

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        public static string Version =>
            ConfigurationManager.AppSettings["version"] ?? "Demo version";

        public static string SaveDataFileName =>
            Path.Combine(CurrentDirectory, GameSettings.SaveDataFileName);

        public static string SettingsFileName =>
            Path.Combine(CurrentDirectory, ConfigurationManager.AppSettings["recordsFilePath"] ?? "Records.json");

        public static string CurrentDirectory => IsDesignMode
                ? Path.GetDirectoryName(GetSourceCodePath())
                : Environment.CurrentDirectory;

        public static string GetSourceCodePath([CallerFilePath] string path = null) => path;
    }
}
