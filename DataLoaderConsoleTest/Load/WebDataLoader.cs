using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DataLoaderConsoleTest
{
    internal abstract class WebDataLoader
    {
        public string URL { get; protected set; }
        public string OutputFileName { get; protected set; }
        public abstract bool IsLoaded { get; }

        public abstract Task WebDownloadDataAsync();
        public abstract Task DeserializeDataFromFileAsync(string fileName = null);
        public abstract void SerializeDataToFileAsync(string fileName = null);

        public bool CheckDowloadedFile()
        {
            var fileInfo = new FileInfo(OutputFileName);
            return fileInfo.Exists && fileInfo.Length == GetFileSize();
        }

        public long GetFileSize()
        {
            var webRequest = WebRequest.Create(URL);
            webRequest.Method = "HEAD";

            using var webResponse = webRequest.GetResponse();
            return long.Parse(webResponse.Headers.Get("Content-Length"));
        }
    }
}
