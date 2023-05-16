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
        public static bool IsDesignMode { get; set; } = true;

        private static IHost host;
        public static IHost Host => host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        protected override async void OnStartup(StartupEventArgs e)
        {
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

        public static string URL =>
            ConfigurationManager.AppSettings["url"];

        public static string Version =>
            ConfigurationManager.AppSettings["version"] ?? "Demo version";

        public static string MetaInfoFileName =>
            ConfigurationManager.AppSettings["metaInfoFileName"];

        public static string DataDirectory =>
            Path.Combine(CurrentDirectory, ConfigurationManager.AppSettings["userDataDirectoryName"]);

        public static string SavesDataDirectory =>
            Path.Combine(DataDirectory, ConfigurationManager.AppSettings["saveDataDirectoryName"]);

        public static string LoadedDataFileName =>
            Path.Combine(DataDirectory, ConfigurationManager.AppSettings["loadedDataFileName"]);

        public static string BrushesNamesFileName =>
            Path.Combine(DataDirectory, ConfigurationManager.AppSettings["brushesNamesFileName"]);

        public static string SettingsFileName =>
            ConfigurationManager.AppSettings["settingsFileName"];

        public static string CurrentDirectory => IsDesignMode
                ? Path.GetDirectoryName(GetSourceCodePath())
                : Environment.CurrentDirectory;

        public static string GetSourceCodePath([CallerFilePath] string path = null) => path;
    }
}
