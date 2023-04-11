﻿using FillwordWPF.Services;
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

        public static IHostBuilder CreateHostBuilder(string[] agrs)
        {
            var host_builder = Host.CreateDefaultBuilder(agrs)
                .UseContentRoot(App.CurrentDirectory)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices(ConfigurateServices);

            return host_builder;
        }

        public static void ConfigureAppConfiguration(HostBuilderContext host, IConfigurationBuilder cfg)
        {
            cfg
                .AddEnvironmentVariables()
                .SetBasePath(App.CurrentDirectory)
                .AddJsonFile("GameSettings.json", optional: false, reloadOnChange: true);
        }

        public static void ConfigurateServices(HostBuilderContext host, IServiceCollection services)
        {
            services
                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<MainMenuViewModel>()
                .AddTransient<SettingsViewModel>()
                .AddTransient<NewGameViewModel>()
                .AddSingleton<GameViewModel>()
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<Func<Type, ViewModel>>(sp => vmt => (ViewModel)sp.GetRequiredService(vmt))
                .ConfigureWritable<GameSettings>(host.Configuration.GetSection(nameof(GameSettings)), "GameSettings.json");
        }
    }
}
