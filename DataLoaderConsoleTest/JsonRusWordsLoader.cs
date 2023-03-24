using System.Collections.Generic;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DataLoaderConsoleTest
{
    internal class JsonRusWordsLoader : DataLoader
    {
        public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public Dictionary<string, WordInfo> Data { get; private set; }
        public JsonObjectSerializer Json { get; private set; }
        public override bool IsLoaded => Data?.Count > 0;

        public JsonRusWordsLoader(string url, string outputFileName, JsonSerializerOptions jsonSerializerOptions = null)
        {
            URL = url;
            OutputFileName = outputFileName;

            Json = new JsonObjectSerializer(jsonSerializerOptions ?? DefaultOptions);
        }

        public override async Task DeserializeDataFromFileAsync()
        {
            Data = await Json.DeserializeAsync<Dictionary<string, WordInfo>>(OutputFileName);
        }

        public override async Task LoadDataAsync()
        {
            await new WebClient().DownloadFileTaskAsync(URL, OutputFileName);
        }

        public override async void SerializeDataToFileAsync(string fileName = null)
        {
            await Json.SerializeAsync(Data, fileName ?? OutputFileName);
        }
    }
}
