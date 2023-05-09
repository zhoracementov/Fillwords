using FillwordWPF.Services.Serializers;
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
        public string Name => name ??= Path.GetFileNameWithoutExtension(FilePath);

        public string FilePath { get; set; }

        public static IEnumerable<Save> GetSaves(ObjectSerializer objectSerializer)
        {
            return Directory
                .GetFiles(App.SavesDataDirectory, $"*{objectSerializer.FileFormat}", SearchOption.TopDirectoryOnly)
                .Select(file => new Save { FilePath = file });
        }

        public Fillword GetFillword()
        {
            return Fillword.Load(FilePath);
        }
    }
}
