using System.Threading.Tasks;

namespace DataLoaderConsoleTest
{
    internal interface IObjectSerializer
    {
        T Deserialize<T>(string fileName);
        Task<T> DeserializeAsync<T>(string fileName);
        void Serialize<T>(T obj, string fileName);
        void SerializeAsync<T>(T obj, string fileName);
    }
}
