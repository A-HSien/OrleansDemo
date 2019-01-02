using AdoNetStorageDemo.IActors.State;
using Orleans;
using System.Threading.Tasks;

namespace AdoNetStorageDemo.IActors
{
    public interface ICustomerGrain : IGrainWithGuidKey
    {
        Task<CustomerState> GetState();
        Task SetState(CustomerState state);
    }
}
