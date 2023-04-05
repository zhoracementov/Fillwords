using Microsoft.Extensions.Configuration;
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
                .ConfigureAppConfiguration((host, cfg) => cfg
                .SetBasePath(App.CurrentDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true))
                .ConfigureServices(App.ConfigurateServices);

            return host_builder;
        }
    }
}
