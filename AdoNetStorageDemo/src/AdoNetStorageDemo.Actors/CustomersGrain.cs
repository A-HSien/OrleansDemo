using AdoNetStorageDemo.IActors;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdoNetStorageDemo.Actors
{
    public class CustomersState
    {
        public List<ICustomerGrain> AllCustomerGrains { get; set; } = new List<ICustomerGrain>();
    }

    [StorageProvider(ProviderName = "OrleansStorage")]
    public class CustomersGrain : Grain<CustomersState>, ICustomersGrain
    {
        private readonly ILogger logger;

        public CustomersGrain(ILogger<CustomersGrain> logger)
        {
            this.logger = logger;
        }

        public Task<IEnumerable<ICustomerGrain>> GetAllCustomers()
        {
            return Task.FromResult(State.AllCustomerGrains.AsEnumerable());
        }

        public async Task AddCustomer(ICustomerGrain newCustomer)
        {
            if (State.AllCustomerGrains.Any(e =>
                e.GetGrainIdentity() == newCustomer.GetGrainIdentity()
            )) return;


            State.AllCustomerGrains.Add(newCustomer);
            await WriteStateAsync();
            return;
        }
    }
}
