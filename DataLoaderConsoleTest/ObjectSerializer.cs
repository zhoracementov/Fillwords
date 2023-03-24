using System.Threading.Tasks;

namespace DataLoaderConsoleTest
{
    internal abstract class ObjectSerializer : IObjectSerializer
    {
        public abstract string FileFormat { get; }

        public abstract T Deserialize<T>(string fileName);
        public abstract Task<T> DeserializeAsync<T>(string fileName);
        public abstract void Serialize<T>(T obj, string fileName);
        public abstract void SerializeAsync<T>(T obj, string fileName);

        protected string GetFileName(string fileName)
        {
            var len = fileName.IndexOf('.');

            return !fileName.Contains(FileFormat)
                ? fileName.Substring(0, len > 0 ? len
                : fileName.Length) + FileFormat : fileName;
        }
    }
}
