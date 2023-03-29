using FillwordWPF.Infrastructure;
using FillwordWPF.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace FillwordWPF.Models
{
    internal class GameSettings
    {
        private static readonly JsonObjectSerializer json = new JsonObjectSerializer(new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        });

        public static string SettingsFileName => ConfigurationManager.AppSettings["recordsFilePath"];

        private readonly Dictionary<string, string> settings;

        private void SetValue(string field, string value)
        {
            settings[field] = value;
            json.Serialize(settings, SettingsFileName);
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
            settings = new Dictionary<string, string>
            {
                { "difficulty", "medium" },
                { "minWordLength", "3" },
                { "saveDataFileName", "data" },
            };

            var fileInfo = new FileInfo(SettingsFileName);

            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                json.Serialize(settings, fileInfo.FullName);
            }

            settings = json.Deserialize<Dictionary<string, string>>(fileInfo.FullName);
        }
    }
}
