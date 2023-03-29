using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace FillwordWPF.Services
{
    internal class BinaryObjectSerializer : ObjectSerializer
    {
        public override string FileFormat => ".bin";

        public override T Deserialize<T>(string fileName)
        {
            using (var fStream = new FileStream(fileName + FileFormat,
                FileMode.Open, FileAccess.Read, FileShare.None))
                return (T)new BinaryFormatter().Deserialize(fStream);
        }

        public override async Task<T> DeserializeAsync<T>(string fileName)
        {
            return await Task.Run(() => Deserialize<T>(fileName));
        }

        public override void Serialize<T>(T obj, string fileName)
        {
            using (var fStream = new FileStream(fileName + FileFormat,
                FileMode.Create, FileAccess.Write, FileShare.None))
                new BinaryFormatter().Serialize(fStream, obj);
        }

        public override async Task SerializeAsync<T>(T obj, string fileName)
        {
            await Task.Run(() => Serialize(obj, fileName));
        }
    }
}
