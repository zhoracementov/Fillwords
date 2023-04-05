using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FillwordWPF.Services
{
    public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

    public class DownloadManager : IDisposable
    {
        private const int bufferSize = 8192;

        private readonly HttpClient httpClient;
        private long fileSize;
        private long? totalFileSize;
        private bool isExist;

        private int connections;

        public string URL { get; }
        public string OutputFilePath { get; }
        public bool IsLoaded { get; private set; }

        public event ProgressChangedHandler ProgressChanged;
        public event EventHandler SuccessfullyDownloaded;

        public DownloadManager(string url, string outputFilePath)
            => (URL, OutputFilePath, httpClient)
            =  (url, outputFilePath, new HttpClient());

        public async Task StartDownload()
        {
            bool isConnect = false;
            do
            {
                try
                {
                    using var sizeRequest = await httpClient.GetAsync(URL, HttpCompletionOption.ResponseHeadersRead);
                    totalFileSize = sizeRequest.Content.Headers.ContentLength;
                    isConnect = true;
                    connections = 0;
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.Message);
                    connections++;
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            } while (!isConnect && connections <= 15);

            if (!isConnect) return;

            isExist = File.Exists(OutputFilePath);
            fileSize = GetFileSize();

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
                await ProcessContentStream(totalFileSize, contentStream);
            }
            else
            {
                if (fileSize != totalFileSize)
                    response.EnsureSuccessStatusCode();
            }
        }

        private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream)
        {
            var buffer = new byte[bufferSize];
            var totalBytesRead = fileSize;
            var isMoreToRead = true;
            var readCount = 0L;

            using (var fileStream = new FileStream(OutputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize, true))
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

            ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage);

            if (progressPercentage.HasValue && progressPercentage.Value == 100)
            {
                SuccessfullyDownloaded(this, new EventArgs());
                IsLoaded = true;
                Dispose();
            }
        }

        private long GetFileSize()
        {
            long? fileSize = null;
            if (isExist)
            {
                fileSize = new FileInfo(OutputFilePath)?.Length;

                var tail = fileSize % bufferSize;
                if (tail != 0) fileSize -= bufferSize;
            }

            return fileSize ?? 0L;
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
