using System;
using System.Collections.Generic;
using System.Text;

namespace FillwordWPF.Services
{
    internal class DownloadDataInput
    {
        public string URL { get; set; }
        public string LoadedDataFileName { get; set; }

        public DownloadDataInput() : this(App.URL, App.LoadedDataFileName)
        {
            //...
        }

        public DownloadDataInput(string url, string loadedDataFileName)
        {
            URL = url;
            LoadedDataFileName = loadedDataFileName;
        }
    }
}
