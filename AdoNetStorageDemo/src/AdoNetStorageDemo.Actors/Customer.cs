using AdoNetStorageDemo.IActors;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace AdoNetStorageDemo.Actors
{
    public class CustomerState
    {
        public string Name { get; set; }
    }

    [StorageProvider(ProviderName = "OrleansStorage")]
    public class Customer : Grain<CustomerState>, ICustomer
    {
        private readonly ILogger logger;

        public Customer(ILogger<Customer> logger)
        {
            this.logger = logger;
        }

        public Task<string> GetName()
        {
            return Task.FromResult(State.Name);
        }

        public async Task SetName(string name)
        {
            State.Name = name;
            await WriteStateAsync();

            var identity = this.GetGrainIdentity();
            logger.LogInformation($"[Customer-{identity.PrimaryKey}] Set name to:{State.Name}");
            return;
        }
    }
}
