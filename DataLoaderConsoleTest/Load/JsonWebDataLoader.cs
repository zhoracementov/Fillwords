using System.Collections;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DataLoaderConsoleTest.Load
{
    internal class JsonWebDataLoader<TCollectionOut> : WebDataLoader where TCollectionOut : ICollection
    {
        public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        private readonly JsonObjectSerializer json;

        public TCollectionOut Data { get; private set; }
        public override bool IsLoaded => Data?.Count > 0;

        public JsonWebDataLoader(string url, string outputFileName, JsonSerializerOptions jsonSerializerOptions = null)
        {
            URL = url;
            OutputFileName = outputFileName;

            json = new JsonObjectSerializer(jsonSerializerOptions ?? DefaultOptions);
        }

        public override async Task DeserializeDataFromFileAsync(string fileName = null)
        {
            Data = await json.DeserializeAsync<TCollectionOut>(fileName ?? OutputFileName);
        }

        public async Task LoadData(string fileName = null)
        {
            if (!CheckDowloadedFile())
                await WebDownloadDataAsync(fileName);

            await DeserializeDataFromFileAsync(fileName);
        }

        public override async Task WebDownloadDataAsync(string fileName = null)
        {
            using var client = new WebClient();
            await client.DownloadFileTaskAsync(URL, fileName ?? OutputFileName);
        }

        public override async void SerializeDataToFileAsync(string fileName = null)
        {
            await json.SerializeAsync(Data, fileName ?? OutputFileName);
        }
    }
}
