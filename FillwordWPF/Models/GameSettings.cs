﻿using FillwordWPF.Infrastructure;
using FillwordWPF.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FillwordWPF.Models
{
    public class GameSettings
    {
        private static readonly Dictionary<string, string> defaultGameSettings = new Dictionary<string, string>
        {
            { "Difficulty", "Easy" },
            { "MinWordLength", "3" },
            { "SaveDataFileName", "Data.json" },
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
            get => Enum.Parse<Difficulty>(settings[nameof(Difficulty)], true);
            set => SetValue(nameof(Difficulty), value.ToString().ToLower());
        }

        public int MinWordLength
        {
            get => int.Parse(settings[nameof(MinWordLength)]);
            set => SetValue(nameof(MinWordLength), value.ToString());
        }

        public string SaveDataFileName
        {
            get => settings[nameof(SaveDataFileName)];
            set
            {
                var textInfo = new System.Globalization.CultureInfo("en-EN").TextInfo;
                var val = textInfo.ToTitleCase(textInfo.ToLower(value));
                SetValue(nameof(SaveDataFileName), val);
            }
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
                settings = defaultGameSettings;
                _ = serializer.SerializeAsync(settings, fileName);
            }
            else
            {
                settings = serializer.Deserialize<Dictionary<string, string>>(fileName);
            }
        }
    }
}
