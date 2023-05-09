using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.Services.Serializers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FillwordWPF.Models
{
    internal class Fillword
    {
        private readonly static ObjectSerializer json = new JsonObjectSerializer();

        public ObservableCollection<FillwordItem> ItemsLinear { get; set; }
        public GameProcessService GameProcessService { get; set; }
        public DateTime InitTime { get; set; }
        public int Size { get; set; }

        public async void SaveAsync()
        {
            await json.SerializeAsync(this, GetName());
        }

        public void Save()
        {
            json.Serialize(this, GetName());
        }

        public static async Task<Fillword> LoadAsync(string fileName)
        {
            return await json.DeserializeAsync<Fillword>(fileName);
        }

        public static Fillword Load(string fileName)
        {
            return json.Deserialize<Fillword>(fileName);
        }

        private string GetName()
        {
            return Path.Combine(App.SavesDataDirectory,
                $"{Environment.UserName} - {InitTime.ToString("dd.MM.yyyy HH.mm.ss")}");
        }
    }
}
