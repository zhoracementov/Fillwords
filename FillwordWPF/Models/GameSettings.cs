using FillwordWPF.Infrastructure;
using FillwordWPF.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FillwordWPF.Models
{
    public class GameSettings
    {
        public static readonly Dictionary<string, string> DefaultSettings = new Dictionary<string, string>
        {
            { "difficulty", "easy" },
            { "minWordLength", "3" },
            { "saveDataFileName", "data" },
        };

        private readonly Dictionary<string, string> settings;
        private readonly string fileName;
        private readonly ObjectSerializer serializer;

        private void SetValue(string field, string value)
        {
            settings[field] = value;
            serializer.Serialize(settings, fileName);
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

        public GameSettings(string fileName)
        {
            serializer = new JsonObjectSerializer(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
            });


            this.fileName = serializer.GetFileName(fileName);
            var fileInfo = new FileInfo(fileName);

            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                settings = DefaultSettings;
                _ = serializer.SerializeAsync(settings, fileName);
            }
            else
            {
                settings = serializer.Deserialize<Dictionary<string, string>>(fileName);
            }
        }
    }
}
