using DataLoaderConsoleTest.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DataLoaderConsoleTest.Load
{
    internal class JsonWebLoader : WebDataLoader
    {
        public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        private readonly JsonObjectSerializer json;

        public object Data { get; private set; }
        public override bool IsLoaded => Data != null && (int)OutputType.GetProperty("Count").GetValue(Data) > 0;
        public override Type OutputType { get; }

        public JsonWebLoader(string url, Type type, string outputFileName, JsonSerializerOptions jsonSerializerOptions = null)
        {
            URL = url;
            OutputFileName = outputFileName;
            OutputType = type;

            json = new JsonObjectSerializer(jsonSerializerOptions ?? DefaultOptions);
        }

        public override async Task DeserializeDataFromFileAsync(string fileName = null)
        {
            Data = await json.DeserializeAsync<Dictionary<string, WordInfo>>(fileName ?? OutputFileName);
        }

        public override async Task WebDownloadDataAsync()
        {
            var client = new WebClient();
            await client.DownloadFileTaskAsync(URL, OutputFileName);
        }

        public override async void SerializeDataToFileAsync(string fileName = null)
        {
            await json.SerializeAsync(Data, fileName ?? OutputFileName);
        }
    }
}
