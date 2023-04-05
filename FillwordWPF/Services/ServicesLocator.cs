using FillwordWPF.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FillwordWPF.Services
{
    internal class ServicesLocator
    {
        public DownloadManager MainWindowViewModel => GetViewModel<DownloadManager>();
        public GameSettings MenuWindowViewModel => GetViewModel<GameSettings>();

        public TService GetViewModel<TService>()
        {
            return App.Host.Services.GetRequiredService<TService>();
        }
    }
}
