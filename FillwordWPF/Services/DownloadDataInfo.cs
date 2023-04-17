using System;
using System.Collections.Generic;
using System.Text;

namespace FillwordWPF.Services
{
    internal class DownloadDataInfo
    {
        public string URL { get; set; }
        public string LoadedDataFileName { get; set; }

        public DownloadDataInfo() : this(App.URL, App.LoadedDataFileName)
        {
            //...
        }

        public DownloadDataInfo(string url, string loadedDataFileName)
        {
            URL = url;
            LoadedDataFileName = loadedDataFileName;
        }
    }
}
