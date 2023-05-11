using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace DataLoaderConsoleTest
{
    internal class Program
    {
        private const string FileName = "data.json";
        private const string OutputFileName = "dataOutput.json";
        private const string URL = @"https://raw.githubusercontent.com/Harrix/Russian-Nouns/main/src/data.json";

        private static void Main(string[] args)
        {
            var downloadDataInput = new DownloadDataInput(URL, FileName);
            var downloadDataService = new DownloadDataService(downloadDataInput);

            _ = downloadDataService.StartDownload();

            var data = GetData();

            data = UserFilterItems(data);

            using var fStream = File.OpenWrite(OutputFileName);
            JsonSerializer.Serialize(fStream, data, DefaultOptions);
        }

        private static WordsData UserFilterItems(WordsData data)
        {
            var ansYes = "1";
            var ansNo = "0";
            var ansStop = "-";

            data = new WordInfoDefinitionConverter(data).Convert();

            var preview = new List<string>();

            while (true)
            {
                var tryPickRes = data.TryPickRandom(out var result, new Random(), x => !preview.Contains(x.Key) && x.Key.Length == 4);

                if (!tryPickRes)
                    break;

                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("[{0}, {1}]", result.Key, result.Value.Definition);
                Console.WriteLine($"Удалить? [{ansYes} - Да, {ansNo} - Нет, {ansStop} - Стоп]");

                var ans = Console.ReadLine();
                if (ans == ansYes)
                {
                    data.Remove(result.Key);
                }
                else if (ans == ansStop)
                {
                    break;
                }

                preview.Add(result.Key);
            }

            return data;
        }

        private static JsonSerializerOptions DefaultOptions => new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            
        };

        private static WordsData GetData()
        {
            using var fStream = File.OpenRead(FileName);
            return JsonSerializer.Deserialize<WordsData>(fStream, DefaultOptions);
        }
    }
}
