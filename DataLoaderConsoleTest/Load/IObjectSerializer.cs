using System.Threading.Tasks;

namespace DataLoaderConsoleTest.Load
{
    internal interface IObjectSerializer
    {
        T Deserialize<T>(string fileName);
        Task<T> DeserializeAsync<T>(string fileName);
        void Serialize<T>(T obj, string fileName);
        Task SerializeAsync<T>(T obj, string fileName);
    }
}
