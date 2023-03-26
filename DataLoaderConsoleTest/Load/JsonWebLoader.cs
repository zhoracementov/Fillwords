using System.Collections;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DataLoaderConsoleTest.Load
{
    internal class JsonWebLoader<TDataOutput> : WebDataLoader where TDataOutput : ICollection
    {
        public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        private readonly JsonObjectSerializer json;

        public TDataOutput Data { get; private set; }
        public override bool IsLoaded => Data?.Count > 0;

        public JsonWebLoader(string url, string outputFileName, JsonSerializerOptions jsonSerializerOptions = null)
        {
            URL = url;
            OutputFileName = outputFileName;

            json = new JsonObjectSerializer(jsonSerializerOptions ?? DefaultOptions);
        }

        public override async Task DeserializeDataFromFileAsync(string fileName = null)
        {
            Data = await json.DeserializeAsync<TDataOutput>(fileName ?? OutputFileName);
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
