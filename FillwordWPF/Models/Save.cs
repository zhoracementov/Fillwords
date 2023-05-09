using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FillwordWPF.Models
{
    internal class Save
    {
        public string name;
        public string Name => name ??= new FileInfo(FilePath).Name;

        public string FilePath { get; set; }

        public static IEnumerable<Save> GetSaves()
        {
            return Directory
                .GetFiles(App.SavesDataDirectory, "*.json", SearchOption.TopDirectoryOnly)
                .Select(file => new Save { FilePath = file });
        }

        public Fillword GetFillword()
        {
            return Fillword.Load(FilePath);
        }
    }
}
