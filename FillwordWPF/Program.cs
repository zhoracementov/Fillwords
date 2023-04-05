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
            _ = new App().Run(new MainWindow());
        }

        public static IHostBuilder CreateHostBuilder(string[] agrs)
        {
            var host_builder = Host.CreateDefaultBuilder(agrs)
                .UseContentRoot(App.CurrentDirectory)
                .ConfigureAppConfiguration((host, cfg) => cfg
                .SetBasePath(App.CurrentDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true))
                .ConfigureServices(ConfigurateServices);

            return host_builder;
        }

        private static void ConfigurateServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
        }
    }
}
