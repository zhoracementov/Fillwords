using DataLoaderConsoleTest.Data;
using DataLoaderConsoleTest.Load;
using DataLoaderConsoleTest.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace DataLoaderConsoleTest
{
    internal class GameSettings
    {
        public static ObjectSerializer Serializer { get; set; }
        public static string SettingsFileName => ConfigurationManager.AppSettings["recordsFilePath"];


        private readonly Dictionary<string, string> settings;

        private void SetValue(string field, string value)
        {
            settings[field] = value;
            Serializer.Serialize(settings, SettingsFileName);
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

            settings = new Dictionary<string, string>
            {
                { "difficulty", "easy" },
                { "minWordLength", "3" },
                { "saveDataFileName", "data" },
            };

            var fileInfo = new FileInfo(SettingsFileName);

            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                Serializer.Serialize(settings, fileInfo.FullName);
            }

            settings = Serializer.Deserialize<Dictionary<string, string>>(fileInfo.FullName);
        }
    }

    internal class Program
    {
        private const string FileName = "data.json";
        private const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        private static void Main()
        {
            var settings = new GameSettings();
            Console.WriteLine(settings.Difficulty);

            settings.Difficulty = Difficulty.Easy;
            Console.WriteLine(settings.Difficulty);
            //var loader = new JsonWebDataLoader<WordsData>(URL, FileName);
            //loader.LoadData().Wait();

            //Console.WriteLine("Старт!");
            //Console.ReadKey();

            //var stopWatch = new Stopwatch();
            //stopWatch.Start();

            //var table = new FillwordTableRandomBuilder(loader.Data, Difficulty.Medium).Build();

            //stopWatch.Stop();

            //Console.WriteLine("RunTime " + stopWatch.Elapsed.Ticks);

            //Console.WriteLine();

            //for (int i = 0; i < table.Size; i++)
            //{
            //    for (int j = 0; j < table.Size; j++)
            //    {
            //        Console.Write(table[i, j].CurrentLetter + " ");
            //    }
            //    Console.WriteLine();
            //}

            Console.ReadKey();
        }
    }
}
