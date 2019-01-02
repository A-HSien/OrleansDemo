using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdoNetStorageDemo.IActors
{
    public interface ICustomersGrain : IGrainWithStringKey
    {
        Task<IEnumerable<ICustomerGrain>> GetAllCustomers();

        Task AddCustomer(ICustomerGrain newCustomer);
    }
}
