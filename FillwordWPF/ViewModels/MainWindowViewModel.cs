using FillwordWPF.Services;
using FillwordWPF.Services.Navigation;
using Microsoft.Extensions.Options;
using System;

namespace FillwordWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private string title = "Error";
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }
        
        private INavigationService navigationService;
        public INavigationService NavigationService
        {
            get => navigationService;
            set => Set(ref navigationService, value);
        }

        public MainWindowViewModel(INavigationService navigationService, IOptions<AppSettings> options)
        {
            NavigationService = navigationService;

            Title = options.Value.Version;

            //App.DownloadManager.ProgressChanged += DownloadManager_ProgressChanged;
            //App.DownloadManager.SuccessfullyDownloaded += DownloadManager_SuccessfullyDownloaded;

            NavigationService.NavigateTo<MainMenuViewModel>();
        }

        //private void DownloadManager_SuccessfullyDownloaded(object sender, EventArgs args)
        //{
        //    Title = _title;
        //}

        //private void DownloadManager_ProgressChanged(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        //{
        //    Title = string.Format("[{0}/{1}]/{2}%", totalFileSize, totalFileSize, progressPercentage);
        //}
    }
}
