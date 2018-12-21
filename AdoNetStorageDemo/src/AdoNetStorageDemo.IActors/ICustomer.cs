using Orleans;
using System.Threading.Tasks;

namespace AdoNetStorageDemo.IActors
{
    public interface ICustomer : IGrainWithGuidKey
    {
        Task<string> GetName();
        Task SetName(string name);
    }
}
