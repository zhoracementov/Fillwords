using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FillwordWPF.Services.Serializers
{
    internal class XmlObjectSerializer : ObjectSerializer
    {
        public override string FileFormat => ".xml";

        public override T Deserialize<T>(string fileName)
        {
            using var fStream = new FileStream(GetFileName(fileName),
                FileMode.Open, FileAccess.Read, FileShare.None);
                    return (T)new XmlSerializer(typeof(T)).Deserialize(fStream);
        }

        public override async Task<T> DeserializeAsync<T>(string fileName)
        {
            return await Task.Run(() => Deserialize<T>(fileName));
        }

        public override void Serialize<T>(T obj, string fileName)
        {
            using var fStream = new FileStream(GetFileName(fileName),
                FileMode.Create, FileAccess.Write, FileShare.None);
                    new XmlSerializer(typeof(T)).Serialize(fStream, obj);
        }

        public override async Task SerializeAsync<T>(T obj, string fileName)
        {
            await Task.Run(() => Serialize(obj, fileName));
        }
    }
}
