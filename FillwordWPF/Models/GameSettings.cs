using FillwordWPF.Infrastructure;
using FillwordWPF.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FillwordWPF.Models
{
    internal class GameSettings
    {
        public static ObjectSerializer Serializer { get; set; }

        private readonly Dictionary<string, string> settings;
        private readonly string fileName;

        private void SetValue(string field, string value)
        {
            settings[field] = value;
            Serializer.Serialize(settings, App.SettingsFileName);
        }

        public Difficulty Difficulty
        {
            get => Enum.Parse<Difficulty>(settings["difficulty"], true);
            set => SetValue("difficulty", value.ToString().ToLower());
        }

        public int MinWordLength
        {
            get => int.Parse(settings["minWordLength"]);
            set => SetValue("minWordLength", value.ToString());
        }

        public string SaveDataFileName
        {
            get => settings["saveDataFileName"];
            set => SetValue("saveDataFileName", value);
        }

        public GameSettings()
        {
            Serializer = new JsonObjectSerializer(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
            });

            fileName = App.SettingsFileName;
            var fileInfo = new FileInfo(fileName);

            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                settings = new Dictionary<string, string>
                {
                    { "difficulty", "easy" },
                    { "minWordLength", "3" },
                    { "saveDataFileName", "data" },
                };

                Serializer.Serialize(settings, fileName);
            }

            settings = Serializer.Deserialize<Dictionary<string, string>>(fileName);
        }
    }
}
