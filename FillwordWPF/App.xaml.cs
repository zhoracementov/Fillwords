using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        public static GameSettingsService GameSettings { get; set; } = new GameSettingsService(SettingsFileName);
        public static DownloadDataService DownloadManager { get; set; } = new DownloadDataService(URL, SaveDataFileName);

        private static IHost host;
        public static IHost Host => host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        protected override async void OnStartup(StartupEventArgs e)
        {
            await DownloadManager.StartDownload();

            IsDesignMode = false;

            base.OnStartup(e);

            await Host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var host = Host;
            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
        }

        public static void ConfigurateServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
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
