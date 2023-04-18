using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FillwordWPF.Services
{
    public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

    internal class DownloadDataService : IDisposable
    {
        private const int bufferSize = 8192;
        private const int ConnectionAttemps = 15;
        private const int ConnectionWaitSeconds = 5;

        private readonly HttpClient httpClient;

        private long fileSize;
        private bool isExist;
        private int connections;

        public string URL { get; }
        public string LoadedDataFileName { get; }
        public bool IsDisposed { get; private set; }
        public long? TotalFileSize { get; private set; }

        public event ProgressChangedHandler ProgressChanged;
        public event ProgressChangedHandler SuccessfullyDownloaded;

        public DownloadDataService(DownloadDataInput downloadDataInfo)
            => (URL, LoadedDataFileName, httpClient)
            =  (downloadDataInfo.URL, downloadDataInfo.LoadedDataFileName, new HttpClient());

        public async Task StartDownload()
        {
            bool isConnect = false;
            do
            {
                try
                {
                    using var sizeRequest = await httpClient.GetAsync(URL, HttpCompletionOption.ResponseHeadersRead);
                    TotalFileSize = sizeRequest.Content.Headers.ContentLength;
                    isConnect = true;
                    connections = 0;
                }
                catch (Exception e)
                {
                    connections++;
                    await Task.Delay(TimeSpan.FromSeconds(ConnectionWaitSeconds));
                }
            } while (!isConnect && connections <= ConnectionAttemps);

            if (!isConnect) return;

            isExist = File.Exists(LoadedDataFileName);
            fileSize = GetFileSize();

            if (fileSize == TotalFileSize)
                return;

            var request = new HttpRequestMessage { RequestUri = new Uri(URL) };
            request.Headers.Range = new RangeHeaderValue(fileSize, null);
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            await DownloadFileFromHttpResponseMessage(response);
        }

        private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response)
        {
            //Console.WriteLine($"[{response.StatusCode}]: {response.RequestMessage}");

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                await ProcessContentStream(TotalFileSize, contentStream);
            }
            else
            {
                if (fileSize != TotalFileSize)
                    response.EnsureSuccessStatusCode();
            }
        }

        private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream)
        {
            var buffer = new byte[bufferSize];
            var totalBytesRead = fileSize;
            var isMoreToRead = true;
            var readCount = 0L;

            using (var fileStream = new FileStream(LoadedDataFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize, true))
            {
                fileStream.Seek(fileSize, SeekOrigin.Begin);

                do
                {
                    int bytesRead = 0;

                    try
                    {
                        bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e.Message);

                        await fileStream.FlushAsync();
                        fileStream.Close();
                        fileStream.Dispose();
                        await StartDownload();
                        return;
                    }


                    if (bytesRead == 0)
                    {
                        isMoreToRead = false;
                        TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                        continue;
                    }

                    await fileStream.WriteAsync(buffer, 0, bytesRead);

                    totalBytesRead += bytesRead;
                    readCount += 1;

                    if (readCount % 100 == 0)
                        TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                }
                while (isMoreToRead);
            }

            //Console.WriteLine(_destinationFilePath.GetMD5());
        }

        private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
        {
            if (ProgressChanged == null)
                return;

            double? progressPercentage = null;
            if (totalDownloadSize.HasValue)
                progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

            ProgressChanged?.Invoke(totalDownloadSize, totalBytesRead, progressPercentage);

            if (progressPercentage.HasValue && progressPercentage.Value == 100)
            {
                SuccessfullyDownloaded?.Invoke(totalDownloadSize, totalBytesRead, progressPercentage);
            }
        }

        private long GetFileSize()
        {
            long? fileSize = null;
            if (isExist)
            {
                fileSize = new FileInfo(LoadedDataFileName)?.Length;

                var tail = fileSize % bufferSize;
                if (tail != 0) fileSize -= bufferSize;
            }

            return fileSize ?? 0L;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;
            httpClient?.Dispose();
        }
    }
}
