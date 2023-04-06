using FillwordWPF.Services.Navigation;
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

        public static IConfiguration Configuration { get; set; }
        public static ServiceProvider ServiceProvider { get; set; }

        public static void ConfigurateServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainMenuViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<NewGameViewModel>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, ViewModel>>(sp => vmt => (ViewModel)sp.GetRequiredService(vmt));

            ServiceProvider = services.BuildServiceProvider();
        }

        public static IHostBuilder CreateHostBuilder(string[] agrs)
        {
            var host_builder = Host.CreateDefaultBuilder(agrs)
                .UseContentRoot(App.CurrentDirectory)
                .ConfigureAppConfiguration((host, cfg) =>
                {
                    cfg = cfg.SetBasePath(App.CurrentDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    Configuration = cfg.Build();
                })
                .ConfigureServices(ConfigurateServices);

            return host_builder;
        }
    }
}
