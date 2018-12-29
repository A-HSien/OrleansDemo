using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdoNetStorageDemo.IActors
{
    public interface ICustomers : IGrainWithStringKey
    {
        Task<IList<ICustomer>> GetAllCustomer();

        Task AddCustomer(ICustomer newCustomer);
    }
}
