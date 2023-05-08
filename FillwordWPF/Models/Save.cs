using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FillwordWPF.Models
{
    internal class Save
    {
        public string FilePath { get; set; }
        public Fillword Fillword => Fillword.Load(FilePath);
        public DateTime InitTime => Fillword.InitTime;

        public static IEnumerable<Save> GetSaves()
        {
            return Directory
                .GetFiles(App.SavesDataDirectory, "*.json", SearchOption.TopDirectoryOnly)
                .Select(file => new Save { FilePath = file });
        }
    }
}
