using System.Threading.Tasks;

namespace FillwordWPF.Services
{
    internal interface IObjectSerializer
    {
        T Deserialize<T>(string fileName);
        Task<T> DeserializeAsync<T>(string fileName);
        void Serialize<T>(T obj, string fileName);
        Task SerializeAsync<T>(T obj, string fileName);
    }
}
