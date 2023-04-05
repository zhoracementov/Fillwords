using FillwordWPF.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FillwordWPF.Services
{
    internal class ServicesLocator
    {
        public DownloadDataService MainWindowViewModel => GetViewModel<DownloadDataService>();
        public GameSettingsService MenuWindowViewModel => GetViewModel<GameSettingsService>();

        public TService GetViewModel<TService>()
        {
            return App.Host.Services.GetRequiredService<TService>();
        }
    }
}
