using System.Threading.Tasks;

namespace FillwordWPF.Services.Serializers
{
    public abstract class ObjectSerializer
    {
        public abstract string FileFormat { get; }

        public abstract T Deserialize<T>(string fileName);
        public abstract Task<T> DeserializeAsync<T>(string fileName);
        public abstract void Serialize<T>(T obj, string fileName);
        public abstract Task SerializeAsync<T>(T obj, string fileName);

        public virtual string GetFileName(string fileName) =>
            !fileName.EndsWith(FileFormat) ? fileName + FileFormat : fileName;
    }
}
