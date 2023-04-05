using System;

namespace FillwordWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private static readonly string _title = "Fillwords " + App.Version;

        private string title = _title;
        public string Title
        {
            get => title;
            private set => Set(ref title, value);
        }

        public MainWindowViewModel()
        {
            App.DownloadManager.ProgressChanged += DownloadManager_ProgressChanged;
            App.DownloadManager.SuccessfullyDownloaded += DownloadManager_SuccessfullyDownloaded;
        }

        private void DownloadManager_SuccessfullyDownloaded(object sender, EventArgs args)
        {
            Title = _title;
        }

        private void DownloadManager_ProgressChanged(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        {
            Title = string.Format("[{0}/{1}]/{2}%", totalFileSize, totalFileSize, progressPercentage);
        }
    }
}
