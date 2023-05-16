using FillwordWPF.Extenstions;
using FillwordWPF.Game;
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
    public class Fillword
    {
        private readonly static ObjectSerializer serializer = new JsonObjectSerializer();

        public ObservableCollection<FillwordItem> ItemsLinear { get; set; }
        public GameProcessService GameProcessService { get; set; }
        public DateTime InitTime { get; set; }
        public int Size { get; set; }

        public async void SaveAsync(ObjectSerializer objectSerializer)
        {
            await objectSerializer.SerializeAsync(this, GetName());
        }

        public void Save(ObjectSerializer objectSerializer)
        {
            objectSerializer.Serialize(this, GetName());
        }

        public static async Task<Fillword> LoadAsync(string fileName, ObjectSerializer objectSerializer)
        {
            return await objectSerializer.DeserializeAsync<Fillword>(fileName);
        }

        public static Fillword Load(string fileName, ObjectSerializer objectSerializer)
        {
            return objectSerializer.Deserialize<Fillword>(fileName);
        }

        public static Fillword CreateRandom(int size, GameProcessService gameProcessService, ObjectSerializer objectSerializer)
        {
            var data = objectSerializer.Deserialize<WordsData>(App.LoadedDataFileName);
            var table = new FillwordTableRandomBuilder(data, size).Build();
            var linear = new ObservableCollection<FillwordItem>(table.AsLinear());

            var newFillword = new Fillword
            {
                ItemsLinear = linear,
                GameProcessService = gameProcessService,
                InitTime = DateTime.Now,
                Size = size
            };

            return newFillword;
        }

        private string GetName()
        {
            return Path.Combine(App.SavesDataDirectory,
                $"{Environment.UserName} - {InitTime.ToString("dd.MM.yyyy HH.mm.ss")}");
        }
    }
}
