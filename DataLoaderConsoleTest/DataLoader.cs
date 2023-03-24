using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DataLoaderConsoleTest
{
    internal abstract class DataLoader
    {
        public string URL { get; protected set; }
        public string OutputFileName { get; protected set; }
        public abstract bool IsLoaded { get; }

        public abstract Task LoadDataAsync();
        public abstract Task DeserializeDataFromFileAsync();
        public abstract void SerializeDataToFileAsync(string fileName = null);

        public bool CheckDowloadedFile()
        {
            var fileInfo = new FileInfo(OutputFileName);
            return fileInfo.Exists && fileInfo.Length == GetFileSize();
        }

        public double GetFileSize()
        {
            var webRequest = WebRequest.Create(URL);
            webRequest.Method = "HEAD";

            using var webResponse = webRequest.GetResponse();
            return double.Parse(webResponse.Headers.Get("Content-Length"));
        }
    }
}
