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
        public IList<ICustomer> AllCustomer { get; set; } = new List<ICustomer>();
    }

    [StorageProvider(ProviderName = "OrleansStorage")]
    public class Customers : Grain<CustomersState>, ICustomers
    {
        private readonly ILogger logger;

        public Customers(ILogger<Customers> logger)
        {
            this.logger = logger;
        }

        public Task<IList<ICustomer>> GetAllCustomer()
        {
            return Task.FromResult(State.AllCustomer);
        }

        public async Task AddCustomer(ICustomer newCustomer)
        {
            if (State.AllCustomer.Any(e =>
                e.GetGrainIdentity() == newCustomer.GetGrainIdentity()
            )) return;

            State.AllCustomer.Add(newCustomer);
            await WriteStateAsync();
            return;
        }
    }
}
