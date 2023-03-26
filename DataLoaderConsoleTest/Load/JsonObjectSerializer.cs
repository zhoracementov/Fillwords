using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataLoaderConsoleTest
{
    internal class JsonObjectSerializer : ObjectSerializer
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public override string FileFormat => ".json";

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

        public override async Task SerializeAsync<T>(T obj, string fileName)
        {
            using var fStream = new FileStream(GetFileName(fileName),
                FileMode.Create, FileAccess.Write, FileShare.None);
            await JsonSerializer.SerializeAsync(fStream, obj, jsonSerializerOptions);
        }
    }
}
