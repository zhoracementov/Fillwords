using FillwordWPF.Models;
using FillwordWPF.Services;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows;

namespace FillwordWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";
        //public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        //{
        //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        //    PropertyNameCaseInsensitive = true,
        //    WriteIndented = true,
        //};

        public static bool IsDesignMode { get; set; } = true;
        public static GameSettings GameSettings { get; private set; }
        public static Task LoadingTask { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            GameSettings = new GameSettings(SettingsFileName);

            base.OnStartup(e);

            //using var manager = new DownloadManager(URL, SaveDataFileName);
            //LoadingTask = manager.StartDownload();
            //LoadingTask.Wait();

            IsDesignMode = false;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        public static string SaveDataFileName =>
            Path.Combine(CurrentDirectory, GameSettings?.SaveDataFileName ?? "data");

        public static string SettingsFileName =>
            Path.Combine(CurrentDirectory, ConfigurationManager.AppSettings["recordsFilePath"] ?? "records");

        public static string CurrentDirectory => IsDesignMode
                ? Path.GetDirectoryName(GetSourceCodePath())
                : Environment.CurrentDirectory;

        public static string GetSourceCodePath([CallerFilePath] string path = null) => path;
    }
}
