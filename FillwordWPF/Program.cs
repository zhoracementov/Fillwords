using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.Services.Navigation;
using FillwordWPF.Services.WriteableOptions;
using FillwordWPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FillwordWPF
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] agrs) => Host
                .CreateDefaultBuilder(agrs)
                .UseContentRoot(App.CurrentDirectory)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices(ConfigurateServices);

        public static void ConfigureAppConfiguration(HostBuilderContext host, IConfigurationBuilder cfg) => cfg
                .SetBasePath(App.CurrentDirectory)
                .AddJsonFile(App.SettingsFileName, optional: false, reloadOnChange: true);

        public static void ConfigurateServices(HostBuilderContext host, IServiceCollection services) => services
                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<MainMenuViewModel>()
                .AddSingleton<SettingsViewModel>()
                .AddSingleton<NewGameViewModel>()
                .AddSingleton<GameViewModel>()
                .AddSingleton<FillwordViewModel>()
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<DownloadDataInput>()
                .AddSingleton<DownloadDataService>()
                .AddSingleton<GameProcessService>()
                .AddSingleton<Func<Type, ViewModel>>(sp => vmt => (ViewModel)sp.GetRequiredService(vmt))
                .ConfigureWritable<GameSettings>(host.Configuration.GetSection(nameof(GameSettings)), App.SettingsFileName);
    }
}
