using System.Collections;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FillwordWPF.Services
{
    internal abstract class WebDataLoader<TCollectionOut> where TCollectionOut : ICollection
    {
        public string URL { get; protected set; }
        public string OutputFileName { get; protected set; }
        public virtual bool IsLoaded => Data?.Count > 0;
        public virtual TCollectionOut Data { get; protected set; }

        public virtual async Task Load(string fileName = null)
        {
            if (!CheckDowloadedFile())
            {
                await WebDownloadDataAsync(fileName);
            }

            await DeserializeDataFromFileAsync(fileName);
        }

        public abstract Task WebDownloadDataAsync(string fileName = null);
        public abstract Task DeserializeDataFromFileAsync(string fileName = null);
        public abstract void SerializeDataToFileAsync(string fileName = null);

        public bool CheckDowloadedFile(string fileName = null)
        {
            fileName = File.Exists(fileName) ? fileName : OutputFileName;
            var fileInfo = new FileInfo(fileName);
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
