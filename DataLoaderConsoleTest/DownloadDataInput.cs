namespace DataLoaderConsoleTest
{
    internal class DownloadDataInput
    {
        public string URL { get; set; }
        public string LoadedDataFileName { get; set; }

        public DownloadDataInput(string url, string loadedDataFileName)
        {
            URL = url;
            LoadedDataFileName = loadedDataFileName;
        }
    }
}
