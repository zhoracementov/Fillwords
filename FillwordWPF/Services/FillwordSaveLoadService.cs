using FillwordWPF.Models;
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

namespace FillwordWPF.Services
{
    internal class FillwordSaveLoadService
    {
        private readonly static JsonObjectSerializer json = new JsonObjectSerializer();

        public IEnumerable<FillwordItem> FillwordItemsLinear { get; set; }
        public GameProcessService GameProcessService { get; set; }

        [JsonIgnore]
        public DateTime InitTime { get; set; }

        public FillwordSaveLoadService(GameProcessService gameProcessService)
        {
            GameProcessService = gameProcessService;
        }

        public async void Save()
        {
            await json.SerializeAsync(this, GetName());
        }

        public static async Task<FillwordSaveLoadService> Load(string fileName)
        {
            return await json.DeserializeAsync<FillwordSaveLoadService>(fileName);
        }

        private string GetName()
        {
            return Path.Combine(App.SavesDataDirectory,
                $"{Environment.UserName} - {InitTime.ToString("dd.MM.yyyy.hh.mm.ss")}");
        }

    }
}
