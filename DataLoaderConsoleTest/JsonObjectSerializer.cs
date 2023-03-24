using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DataLoaderConsoleTest
{
    internal class JsonObjectSerializer : ObjectSerializer
    {
        public override string FileFormat { get; } = ".json";

        public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        private readonly JsonSerializerOptions jsonSerializerOptions;

        public JsonObjectSerializer(JsonSerializerOptions jsonSerializerOptions = null)
        {
            this.jsonSerializerOptions = jsonSerializerOptions ?? JsonSerializerOptions.Default;
        }

        public override T Deserialize<T>(string fileName)
        {
            using var fStream = new FileStream(fileName,
                FileMode.Open, FileAccess.Read, FileShare.None);
            return JsonSerializer.Deserialize<T>(fStream, jsonSerializerOptions);
        }

        public override async Task<T> DeserializeAsync<T>(string fileName)
        {
            using var fStream = new FileStream(fileName,
                FileMode.Open, FileAccess.Read, FileShare.None);
            return await JsonSerializer.DeserializeAsync<T>(fStream, jsonSerializerOptions);
        }

        public override void Serialize<T>(T obj, string fileName)
        {
            using var fStream = new FileStream(GetFileName(fileName),
                FileMode.Create, FileAccess.Write, FileShare.None);
            JsonSerializer.Serialize(fStream, obj, jsonSerializerOptions);
        }

        public override async void SerializeAsync<T>(T obj, string fileName)
        {
            using var fStream = new FileStream(GetFileName(fileName),
                FileMode.Create, FileAccess.Write, FileShare.None);
            await JsonSerializer.SerializeAsync(fStream, obj, jsonSerializerOptions);
        }
    }
}
